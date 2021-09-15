namespace Script.Skill
{
    public enum SkillKind
    {
        /*
         * 物理招式
         * 物理招式计算攻击方的攻击与防御方的防御。
         * 伤害受反射盾减少。受双倍奉还反弹。直接扣除HP的招式不受以上限制。
         * 特殊招式
         * 特殊招式计算攻击方的特攻与防御方的特防
         * 伤害受光之壁减少。受镜面反射反弹。直接扣除HP的招式不受以上限制。
         * 变化招式
         * 不造成伤害的招式被归为变化类。
         */

        Physical, // 物理招式
        Special, // 特殊招式
        Transformation, //变化招式
    }
}