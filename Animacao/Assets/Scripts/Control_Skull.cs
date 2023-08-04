using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Control_Skull : MonoBehaviour
{
    
    //------Vari�veis P�blicas-----
    //Vari�veis que receber�o os efeitos sonoros
    public AudioClip walking, attack, injured, groggy, soundBackground;
    //Vari�vel que receber� o GameObject Animator do personagem;
    public Animator anmtAnimation;
    //------Vari�veis Privadas------
    //Essa vari�vel receber� o deslocamento no eixo horizontal 
    float horizontalDisplacement;
    //Essas vari�veis limitaram o deslocamento horizontal
    float minimumHorizontal, maximumHorizontal;
    //[SerializeField] exibe uma vari�vel Privada na gui Inspector do Unity
    [SerializeField]
    //Vari�vel que definir� a dire��o que o personagem est� voltado
    float direction;
    //Vari�vel que receber� o Rigidbody2D do personagem
    Rigidbody2D rdbSkull;
    //Receber� o componente Animator do personagem
    //Animator anmtAnimation;
    //Vari�vel que emitir� o som selecionado
    AudioSource emissionSound;
    //Vari�vel que definir� a velocidade de deslocamento
    float velocity;
    //Essa vari�vel receber� a posi��o inicial no eixo Y do personagem antes do salto
    float startingPosition;
    //Essa vari�vel receber� a posi��o atual  no eixo Y do personagem durante o salto
    float currentPosition;
    //Essa vari�vel receber� a altura m�xima atingida pelo personagem durante o salto
    float totalHeight;
    //Essas vari�veis receber�o a situa��o do salto do personagem 
    bool isJumpedUp, isJumpedDown;

    // Start is called before the first frame update
    void Start()
    {
        minimumHorizontal = -8.5f;
        maximumHorizontal = 8.5f;
        velocity = 5;
        isJumpedUp = false;
        //Inicialize as vari�veis com as refer�ncia dos objetos : Rigidbody 2D e AudioSource
        rdbSkull = GetComponent<Rigidbody2D>();
        emissionSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Essa condicional controlar� o salto durante a subida 
        if(isJumpedUp == true && isJumpedDown == false)
        {
            /*A vari�vel currentPosition receber� a posi��o atual do * personagem no eixo Y formatada com 3 casas decimais*/
            currentPosition = (float)Math.Round((double)transform.position.y, 3);
            if(currentPosition > totalHeight ) 
            { 
             totalHeight = currentPosition;

            }else if (currentPosition < totalHeight)
            {
                isJumpedUp = false;
                isJumpedDown = true;
                anmtAnimation.SetBool("UpJump", false);
                anmtAnimation.SetBool("JumpingDown", true);
            }
        }
      //Essa condicional controlar� o salto durante a descida
      if(isJumpedUp == false && isJumpedDown == true)
        {
            /*A vari�vel currentPosition receber� a posi��o atual do * personagem no eixo Y formatada com 3 casas decimais*/
            currentPosition = (float)Math.Round((double)transform.position.y, 3);
            if (currentPosition <= startingPosition)
            {
                isJumpedDown = false;
                anmtAnimation.SetBool("JumpingDown", false);
                anmtAnimation.SetBool("Idle", true);
            }       
      }

        //Essa condicional controla o ataque do personagem(Ctrl)
        if (Input.GetButtonDown("Fire1"))
        {
            anmtAnimation.SetBool("Attack", true);
            anmtAnimation.SetBool("Idle", false);
            anmtAnimation.SetBool("Walking", false);
            PlaySound(attack);
        }
        else
        {
            anmtAnimation.SetBool("Attack", false);
        }
        //Essa condicional controla o estado de grogue do personagem (G)
        if (Input.GetKey(KeyCode.G))
        {
            anmtAnimation.SetBool("Groggy", true);
            anmtAnimation.SetBool("Idle", false);
            anmtAnimation.SetBool("Walking", false);
            PlaySound(groggy);
        }
        else
        {
            anmtAnimation.SetBool("Groggy", false);
        }

        //Essa condicional controlar� o estado de ferido do personagem (F)
        if (Input.GetKeyDown(KeyCode.F))
        {

            anmtAnimation.SetBool("Injured", true);
            anmtAnimation.SetBool("Idle", false);
            anmtAnimation.SetBool("Walking", false);
        }
        else
        {
            anmtAnimation.SetBool("Injured", false);
        }

        //Essa condicional controlar� o estado de morte do personagem (F)
        if (Input.GetKeyDown(KeyCode.M))
        {

            anmtAnimation.SetBool("Dying", true);
            anmtAnimation.SetBool("Idle", false);
            anmtAnimation.SetBool("Walking", false);
        }
        else
        {
            anmtAnimation.SetBool("Dying", false);
        }

        //Essa condi��o controla o pulo do personagem 
        if (Input.GetButtonDown("Jump"))
        {
            //Guarda a posi��o atual do personagem 
            startingPosition = (float)Math.Round((double)transform.position.y, 5);

            //Pulo
            rdbSkull.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
            anmtAnimation.SetBool("UpJump", true);
            anmtAnimation.SetBool("Idle", false);
            anmtAnimation.SetBool("Walking", false);

            totalHeight = startingPosition - 0.001f;
            isJumpedUp = true;
            isJumpedDown = false;
        }

        //Deslocamento do personagem
        transform.Translate(horizontalDisplacement, 0, 0);
    }
    void FixedUpdate()
    {
        horizontalDisplacement = (Input.GetAxis("Horizontal") * Time.deltaTime) * velocity;

        //Testa se o limite de deslocamento para a direita j� foi atingido e reposiciona o personagem
        if(transform.position.x > maximumHorizontal)
        {
            transform.position = new Vector2(maximumHorizontal, transform.position.y);
        }

        //Testa se o limite de deslocamento para a esquerda j� foi atingido e reposiciona o personagem
        if (transform.position.x < minimumHorizontal)
        {
            transform.position = new Vector2(minimumHorizontal, transform.position.y);
        }
        
        //Essa condicional controla o movimento horizontal do personagem
        if (Input.GetAxis("Horizontal") != 0)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                direction = 2.0f;
            }
            else
            {
                direction = -2.0f;
            }

            transform.localScale = new Vector3(direction, 2.0f, 0f);
            anmtAnimation.SetBool("Walking", true);
            anmtAnimation.SetBool("Idle", false);
            PlaySound(walking);
        }
        else
        {
            anmtAnimation.SetBool("Walking", false);
            anmtAnimation.SetBool("Idle", true);
            emissionSound.Stop();
        }

    }

    void PlaySound(AudioClip audioPlay)
    {
        if(soundBackground == null)
        {
            soundBackground = audioPlay;
            emissionSound.PlayOneShot(audioPlay);
        }else if( soundBackground != audioPlay)
        {
            if(!emissionSound.isPlaying)
            {
                emissionSound.PlayOneShot(audioPlay);
            }
            else
            {
                emissionSound.Stop();
                emissionSound.PlayOneShot(audioPlay);
            }
            soundBackground = audioPlay;
        }else if(soundBackground == audioPlay)
        {
            if(emissionSound.isPlaying == false)
            {
                emissionSound.PlayOneShot(audioPlay);
            }
        }
    }
}
