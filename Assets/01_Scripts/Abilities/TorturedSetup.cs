public class TorturedSetup : ChickenAbilitySetup
{
    ChickenAbilitySetup[] allMyAbilities;

    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        allMyAbilities = GetComponents<ChickenAbilitySetup>();
    }

    protected override void Update()
    {
        base.Update();

        foreach (ChickenAbilitySetup ability in allMyAbilities)
        {
            if (ability.CanCall()) ability.Call();
        }
    }
}