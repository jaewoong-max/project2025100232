using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }    // 싱글톤 인스턴스

    private AudioSource audioBGM = null;        // BGM 오디오 소스

    private const string pathBGM = "Sounds/BGM/";    // BGM 리소스 경로
    private const string pathSE = "Sounds/SE/";      // SE 리소스 경로

    private Dictionary<string, AudioClip> dictBGM = new Dictionary<string, AudioClip>();    // BGM 딕셔너리
    private Dictionary<string, AudioClip> dictSE = new Dictionary<string, AudioClip>(); // SE 딕셔너리    

    private float fBGMVolume = 1.0f;        // BGM 볼륨
    private float fSEVolume = 1.0f;         // SE 볼륨

    private void Awake()
    {
        if (Instance != null && Instance != this)   // 이미 인스턴스가 존재하고 자기 자신이 아니면
        {
            Destroy(gameObject);        // 중복 프리팹 제거
            return;
        }
        Instance = this;                // 싱글톤 인스턴스 설정
        DontDestroyOnLoad(gameObject);  // 씬 넘어가도 유지

        fBGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);       // 이전에 저장된 볼륨 불러오기, 없으면 0.5f 기본값
        fSEVolume = PlayerPrefs.GetFloat("SEVolume", 0.5f);         // 이전에 저장된 볼륨 불러오기, 없으면 0.5f 기본값

        audioBGM = gameObject.AddComponent<AudioSource>();   // BGM 재생용 오디오 소스 추가
    }

    // BGM 재생 (파일을 읽어와서 재생)
    public void PlayBGM(string key, bool _bLoop = true)
    {
        if (dictBGM.ContainsKey(key) == false)      // 딕셔너리에 키가 없으면
        {
            AudioClip clip = Resources.Load<AudioClip>(pathBGM + key);  // 리소스에서 오디오 클립 로드
            if (clip == null)       // 로드 실패 시 종료
                return;
            dictBGM.Add(key, clip);     // 딕셔너리에 추가
        }

        PlayBGM(dictBGM[key], _bLoop);  // 오버로드 된 PlayBGM 호출 (AudioClip으로 재생)
    }

    // BGM 재생 (AudioClip으로 재생)
    public void PlayBGM(AudioClip _audio, bool _bLoop = true)
    {
        // AudioClip이 Null 이거나 AudioSource컴포넌트가 없으면 종료
        // 이미 재생중인 BGM과 동일한 클립이면 종료
        if (_audio == null || audioBGM == null || audioBGM.clip == _audio)
            return;

        audioBGM.clip = _audio;         // 오디오 소스에 클립 설정
        audioBGM.loop = _bLoop;         // 루프 설정
        audioBGM.volume = fBGMVolume;   // 볼륨 설정
        audioBGM.Play();                // 재생
    }

    // SE 재생 (파일을 읽어와서 재생)
    public void PlaySE(string key)
    {
        if (dictSE.ContainsKey(key) == false)       // 딕셔너리에 키가 없으면
        {
            AudioClip clip = Resources.Load<AudioClip>(pathSE + key);   // 리소스에서 오디오 클립 로드
            if (clip == null)   // 로드 실패 시 종료
                return;
            dictSE.Add(key, clip);  // 딕셔너리에 추가
        }

        PlaySE(dictSE[key]);    // 오버로드 된 PlaySE 호출 (AudioClip으로 재생)
    }

    // SE 재생 (AudioClip으로 재생)
    // _fTime: 재생 시간 (0.0f이면 클립 길이만큼 재생)
    public void PlaySE(AudioClip _audioclip, float _fTime = 0.0f)
    {
        if (_audioclip == null) // AudioClip이 Null 이면 종료
            return;

        GameObject obj = new GameObject();          // 새로운 게임 오브젝트 생성
        obj.transform.SetParent(this.transform);    // 사운드 매니저의 자식으로 설정
        AudioSource source = obj.AddComponent<AudioSource>();   // 오디오 소스 컴포넌트 추가

        source.clip = _audioclip;   // 오디오 클립 설정
        source.loop = false;        // 루프 비활성화
        source.volume = fSEVolume;  // 볼륨 설정

        source.Play();      // 재생

        if (_fTime == 0.0f)
            Destroy(obj, source.clip.length);   // 클립 길이만큼 후에 오브젝트 파괴
        else
            Destroy(obj, _fTime);   // 지정된 시간 후에 오브젝트 파괴
    }
}
