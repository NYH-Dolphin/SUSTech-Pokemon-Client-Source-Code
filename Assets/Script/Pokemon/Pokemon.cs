
using System.Collections.Generic;

public abstract class Pokemon
{

    protected  string name; // 宝可梦名字
    protected  int id;// 宝可梦id
    protected Genre genre; //宝可梦种类
    protected  int exp; // 宝可梦经验
    protected List<Skill> skills; //宝可梦的所有技能

    protected int firstEvo = 20;
    protected int secondEvo = 60;
    
    

    // 基础属性
    protected  double HP;//生命值
    protected  double ATK;//攻击
    protected  double DEF;//防御
    protected  double SPEED;//速度
    protected  double SATK;//special ATK
    protected  double SDEF;//special DEF
    protected  double currentHP;// 当前血量


    public int GetLevel()
    {
        return 0;
    }
    
    


}
