namespace jigsaw.Engine
{
    abstract class AbstractParser
    {
        public string Raw { get; protected set; }

        abstract public void Parse();
    }
}
