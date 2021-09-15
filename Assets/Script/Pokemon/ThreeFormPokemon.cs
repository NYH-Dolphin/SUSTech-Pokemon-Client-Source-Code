namespace Script.Pokemon
{
    public abstract class ThreeOrderPokemon : Pokemon
    {
        private string _firstForm;
        private string _secondForm;
        private string _thirdForm;

        protected void InitName(string first, string second, string third)
        {
            _firstForm = first;
            _secondForm = second;
            _thirdForm = third;
        }

        public override string GetName()
        {
            return (GetLevel() >= 40) ? (GetLevel() >= 60) ? _thirdForm : _secondForm : _firstForm;
        }
    }
}