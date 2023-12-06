using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace Colosoft.Mapping.Xml
{
    internal class XmlMappingDataSource : IXmlMappingDataSource
    {
        public XmlMappingDataSource(IXPathNavigable root)
        {
            var navigator = this.Root.CreateNavigator();
            this.Root = root;
            this.Name = navigator.Name;
            var fields = this.GetFields().ToList();
            this.Schema = new XmlMappingDataSourceSchema(fields);
        }

        public IXPathNavigable Root { get; }

        public string Name { get; }

        public IMappingDataSourceSchema Schema { get; }

        public Task<IEnumerable<IMappingDataSourceRecord>> GetRecords(CancellationToken cancellationToken)
        {
            var navigator = this.Root.CreateNavigator();
            var document = new XmlDocument();
            navigator.MoveToRoot();
            document.Load(navigator.ReadSubtree());
            var nodes = document.DocumentElement.ChildNodes;
            var records = new List<IMappingDataSourceRecord>();

            for (int i = 0; i <= nodes.Count; i++)
            {
                records.Add(new XmlMappingDataSourceRecord(nodes[i], this.Schema));
            }

            return Task.FromResult<IEnumerable<IMappingDataSourceRecord>>(records);
        }

        private IEnumerable<IMappingDataSourceSchemaField> GetFields()
        {
            var navigator = this.Root.CreateNavigator();
            var document = new XmlDocument();
            navigator.MoveToRoot();
            document.Load(navigator.ReadSubtree());
            var nodes = document.DocumentElement.ChildNodes;

            for (int i = 0; i < nodes.Count; i++)
            {
                yield return new XmlMappingDataSourceSchemaField(nodes[0].ChildNodes[i].Name, typeof(string));
            }
        }
    }
}
