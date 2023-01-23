using UnityEngine;

public class SpaceSoundtrackManager : MonoBehaviour
{
    [SerializeField] private SoundController preSpaceSoundtrack;
    [SerializeField] private SoundController spaceSoundtrack;
    [Tooltip("Height where the rocket transitions from pre-space to space")]
    [SerializeField] private float spaceTransitionHeight;
    [Tooltip("Height where space soundtrack is fully playing")]
    [SerializeField] private float maxHeight;
    [SerializeField] private LiftoffSceneManager liftoffSceneManager;

    private SpaceStage currentSpaceStage = SpaceStage.PreSpace;

    
    private enum SpaceStage
    {
        PreSpace,
        Space
    }


    private void Start()
    {
        spaceSoundtrack.enabled = false;
    }

    private void Update()
    {
        if (!liftoffSceneManager.IsFlying)
            return;

        switch (currentSpaceStage)
        {
            case SpaceStage.PreSpace:
            {
                if (liftoffSceneManager.currentHeight > spaceTransitionHeight)
                {
                    currentSpaceStage = SpaceStage.Space;
                    preSpaceSoundtrack.enabled = false;
                    spaceSoundtrack.enabled = true;
                }
                else
                {
                    float value = liftoffSceneManager.currentHeight / spaceTransitionHeight; 
                    preSpaceSoundtrack.SetParameter("Pre-Space Height", value);
                }
                break;
            }
            case SpaceStage.Space:
            {
                float value = (liftoffSceneManager.currentHeight - spaceTransitionHeight) / (maxHeight - spaceTransitionHeight); 
                spaceSoundtrack.SetParameter("Height", value);
                break;
            }
        }

    }
}
