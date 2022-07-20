using UnityEngine;

public class FightMessage
{
    private FightCode _code;
    private int _currPokemon;
    private int _currSkill;
    private int _currLevel;
    private int _language;

    public int CurrLevel
    {
        get => _currLevel;
        set => _currLevel = value;
    }

    public FightMessage(FightCode code)
    {
        _code = code;
        _language = PlayerPrefs.GetString("language", "EN") == "CN" ? 0 : 1;
        Language = _language;
    }

    public FightCode Code
    {
        get => _code;
        set => _code = value;
    }

    public int Language
    {
        get => _language;
        set => _language = value;
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