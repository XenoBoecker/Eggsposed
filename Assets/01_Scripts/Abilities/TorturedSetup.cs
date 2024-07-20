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

        chicken.Call();
    }
}