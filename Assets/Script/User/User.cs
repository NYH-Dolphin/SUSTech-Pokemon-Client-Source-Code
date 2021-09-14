using System.Collections.Generic;
using Script.Pokemon;

public class User
{
    protected string name;
    protected int id;
    protected int exp;
    protected string account;
    protected string password;
    protected int protrate;

    protected HashSet<Pokemon> pokemons;
    protected Package pocket;
    protected int coins;
    protected int pokemonBall;
    protected Pokemon[] fightPokemons;
} 
