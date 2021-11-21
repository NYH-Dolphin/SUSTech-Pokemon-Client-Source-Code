public class FightMessage
{
    private FightCode _code;
    private int _currPokemon;
    private int _currSkill;
    private int _currLevel;

    public int CurrLevel
    {
        get => _currLevel;
        set => _currLevel = value;
    }

    public FightMessage(FightCode code)
    {
        _code = code;
    }

    public FightCode Code
    {
        get => _code;
        set => _code = value;
    }

    public int CurrPokemon
    {
        get => _currPokemon;
        set => _currPokemon = value;
    }

    public int CurrSkill
    {
        get => _currSkill;
        set => _currSkill = value;
    }
}