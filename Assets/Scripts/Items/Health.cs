public class Health : PickableItem
{
    public int health;
    public Player player;
    
    public override void RunAction()
    {
        player.AddHealth(health);
        player.backpack.RemovePickableItem(this, false);
        Destroy(this);
    }

    public override bool HasAction()
    {
        return true;
    }
}
