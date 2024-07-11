public class HiveMindSetup : ChickenAbilitySetup
{
    Chicken[] allChickens;
    
    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        allChickens = FindObjectsOfType<Chicken>();

        foreach (Chicken chick in allChickens)
        {
            chick.SetControlledByPlayer(true);
        }

        bcc.OnFinishBreeding += GiveUpControl;
    }

    private void GiveUpControl()
    {
        foreach (Chicken chick in allChickens)
        {
            chick.SetControlledByPlayer(false);
        }
    }
}