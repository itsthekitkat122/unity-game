using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; set; }

    public AudioSource ShootingChannel;

    public AudioClip P1911Shot;
    public AudioClip M4Shot;

    
    public AudioSource reloadingSoundM4;
    public AudioSource reloadingSound1911;


    public AudioSource emptyMagazineSound1911;
    

    private void Awake()
    {
        if(Instance != null && Instance!= this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        } 


    }

    public void PlayShootingSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol1911:
                ShootingChannel.PlayOneShot(P1911Shot);
                break;
            case Weapon.WeaponModel.M4:
                ShootingChannel.PlayOneShot(M4Shot);
                break;
        }
    }

    public void PlayReloadSound(Weapon.WeaponModel weapon)
    {
           switch (weapon)
        {
            case Weapon.WeaponModel.Pistol1911:
                reloadingSound1911.Play();
                break;
            case Weapon.WeaponModel.M4:
                reloadingSoundM4.Play();
                break;
        } 
    }


}