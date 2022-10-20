namespace ScriptLoader.Core
{
    public class Script
    {
        public Script(string name, string body)
        {
            Name = name;
            Body = body;
        }
        public Script(string body)
        {
            Body = body;
            Name = Guid.NewGuid().ToString()+".js";
        }
        public string Name { get; set; }
        public string Body { get; set; }
    }
}