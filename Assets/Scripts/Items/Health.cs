using UnityEngine;

public class Health : PickableItem
{
    public int health;
    private Player _player;

    public override void Pick(GameObject backpack)
    {
        base.Pick(backpack);
        _player = backpack.GetComponentInParent<Player>();
    }
    
    public override void RunAction()
    {
        if (_player.HasMaxSpeed())
        {
            return;
        }
        _player.AddHealth(health);
        _player.backpack.RemovePickableItem(this, false);
        Destroy(this.gameObject);
    }

    public override bool HasAction()
    {
        return true;
    }
}
