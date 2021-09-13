public class Skill
{
    protected string name;
    protected int id;
    protected int unlockLevel;//解锁等级
    protected int duration; //技能时效

    public Skill(int unlockLevel)
    {
        this.unlockLevel = unlockLevel;
    }

    protected Genre genre;
    
    
}

