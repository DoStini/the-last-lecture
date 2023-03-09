using Random = System.Random;
using System.Threading.Tasks;
using UnityEngine;

public class Speed : PickableItem
{
    public float boost;
    public float minBoost;
    public float maxBoost;

    public int minDuration;
    public int maxDuration;

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
    }
    
    public override void RunAction()
    {
        float actualBoost = _player.BoostSpeed(boost);
        _player.backpack.RemovePickableItem(this, false);
        Task.Delay(duration * 1000).ContinueWith((t) => UndoAction(actualBoost));
        Destroy(this.gameObject);
    }

    public override bool HasAction()
    {
        return true;
    }

    public override void Randomize()
    {
        boost = (float)(new Random().NextDouble() * (maxBoost - minBoost) + minBoost);
        duration = new Random().Next(minDuration, maxDuration);
    }
}
