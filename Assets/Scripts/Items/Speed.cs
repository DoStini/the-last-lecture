using System.Threading.Tasks;
using UnityEngine;

public class Speed : PickableItem
{
    public float boost;
    public int duration;
    private Player _player;

    public override void Pick(GameObject backpack)
    {
        base.Pick(backpack);
        _player = backpack.GetComponentInParent<Player>();
    }

    private void UndoAction(float actualBoost)
    {
        _player.DecreaseSpeed(actualBoost);
        Destroy(this);
    }
    
    public override void RunAction()
    {
        float actualBoost = _player.BoostSpeed(boost);
        _player.backpack.RemovePickableItem(this, false);
        Task.Delay(duration * 1000).ContinueWith((t) => UndoAction(actualBoost));
    }

    public override bool HasAction()
    {
        return true;
    }
}
