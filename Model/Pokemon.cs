namespace PokemonWebApi.Model
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool HasMega { get; set; } = false;
        public int? Number { get; set; }
        public string? Type1 { get; set; }
        public string? Type2 { get; set; }
        public string? Species { get; set; }
        public string? Description { get; set; }
        public string? NameOfImage { get; set; }

    }
}
