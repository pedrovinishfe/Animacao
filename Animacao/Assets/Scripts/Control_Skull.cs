using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Control_Skull : MonoBehaviour
{
    
    //------Variáveis Públicas-----
    //Variáveis que receberão os efeitos sonoros
    public AudioClip walking, attack, injured, groggy, soundBackground;
    //Variável que receberá o GameObject Animator do personagem;
    public Animator anmtAnimation;
    //------Variáveis Privadas------
    //Essa variável receberá o deslocamento no eixo horizontal 
    float horizontalDisplacement;
    //Essas variáveis limitaram o deslocamento horizontal
    float minimumHorizontal, maximumHorizontal;
    //[SerializeField] exibe uma variável Privada na gui Inspector do Unity
    [SerializeField]
    //Variável que definirá a direção que o personagem está voltado
    float direction;
    //Variável que receberá o Rigidbody2D do personagem
    Rigidbody2D rdbSkull;
    //Receberá o componente Animator do personagem
    //Animator anmtAnimation;
    //Variável que emitirá o som selecionado
    AudioSource emissionSound;
    //Variável que definirá a velocidade de deslocamento
    float velocity;
    //Essa variável receberá a posição inicial no eixo Y do personagem antes do salto
    float startingPosition;
    //Essa variável receberá a posição atual  no eixo Y do personagem durante o salto
    float currentPosition;
    //Essa variável receberá a altura máxima atingida pelo personagem durante o salto
    float totalHeight;
    //Essas variáveis receberão a situação do salto do personagem 
    bool isJumpedUp, isJumpedDown;

    // Start is called before the first frame update
    void Start()
    {
        minimumHorizontal = -8.5f;
        maximumHorizontal = 8.5f;
        velocity = 5;
        isJumpedUp = false;
        //Inicialize as variáveis com as referência dos objetos : Rigidbody 2D e AudioSource
        rdbSkull = GetComponent<Rigidbody2D>();
        emissionSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Essa condicional controlará o salto durante a subida 
        if(isJumpedUp == true && isJumpedDown == false)
        {
            /*A variável currentPosition receberá a posição atual do * personagem no eixo Y formatada com 3 casas decimais*/
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
      //Essa condicional controlará o salto durante a descida
      if(isJumpedUp == false && isJumpedDown == true)
        {
            /*A variável currentPosition receberá a posição atual do * personagem no eixo Y formatada com 3 casas decimais*/
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

        //Essa condicional controlará o estado de ferido do personagem (F)
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

        //Essa condicional controlará o estado de morte do personagem (F)
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

        //Essa condição controla o pulo do personagem 
        if (Input.GetButtonDown("Jump"))
        {
            //Guarda a posição atual do personagem 
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

        //Testa se o limite de deslocamento para a direita já foi atingido e reposiciona o personagem
        if(transform.position.x > maximumHorizontal)
        {
            transform.position = new Vector2(maximumHorizontal, transform.position.y);
        }

        //Testa se o limite de deslocamento para a esquerda já foi atingido e reposiciona o personagem
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
