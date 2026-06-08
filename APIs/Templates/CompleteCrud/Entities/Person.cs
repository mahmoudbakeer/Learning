namespace CompleteCrud.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static ValueTask<Person> BindAsync(HttpContext context)
        {
            var idstr = context.Request.Query["id"];
            var namestr = context.Request.Headers["name"];
            if (int.TryParse(idstr, out int id))
            {
                return new ValueTask<Person?>(new Person { Id = id, Name = namestr });
            }
            else
                return new ValueTask<Person?>(Task.FromResult<Person?>(null));
        }
    }
}
