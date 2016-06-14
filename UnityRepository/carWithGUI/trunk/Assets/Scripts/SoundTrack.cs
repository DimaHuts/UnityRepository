using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public delegate void ProcessingSound(SoundTrack track);
    public delegate void CompleteSound(SoundTrack track, float time_offset);
    public delegate void DestroySound(SoundTrack track, bool atEndOfSound, float time_offset);

    public class SoundTrack : MonoBehaviour {
	
	public static bool IsHandlerAttached(ProcessingSound _event, ProcessingSound _handler){
		return ((IList)(_event.GetInvocationList())).Contains(_handler);
	}
	public static bool IsHandlerAttached(CompleteSound _event, CompleteSound _handler){
		return ((IList)(_event.GetInvocationList())).Contains(_handler);
	}
	public static bool IsHandlerAttached(DestroySound _event, DestroySound _handler){
		return ((IList)(_event.GetInvocationList())).Contains(_handler);
	}
	
	public static SoundTrack PlaySound(AudioClip _clip, float _volume = 1, float _pitch = 1, 
	                                   float _loop_time = 0, float _delay_time = 0){

		GameObject _go = new GameObject("[~SOUND]"+_clip.name);
		SoundTrack _track=PlaySound(_go, _clip, _volume, _pitch, _loop_time, _delay_time);
		_track.ownGameobject = true;

		#if UNITY_EDITOR
		if(!UnityEditor.EditorApplication.isPlaying){
			_go.hideFlags=HideFlags.DontSave;
		}
		#endif

		return _track;
	}
	
	public static SoundTrack PlaySound(GameObject _target, AudioClip _clip, float _volume = 1, float _pitch = 1, 
	                                   float _loop_time = 0, float _delay_time = 0){

		SoundTrack _track = _target.AddComponent<SoundTrack>();
		AudioSource _source=_track.audioSrc = _target.AddComponent<AudioSource>();

		#if UNITY_EDITOR
		if(!UnityEditor.EditorApplication.isPlaying){
			_track.hideFlags=HideFlags.DontSave;
			_source.hideFlags=HideFlags.DontSave;
		}
		#endif

		_source.clip = _clip;
		_source.volume = _volume;
		_source.pitch = _pitch; // recomended limit -3 , 3
		
		_track._delay_time=_delay_time;
		_track._length=_clip.length;
		
		if(_loop_time!=0){
			_source.loop = true;
			if(_loop_time<0){
				_track._loop_time=float.PositiveInfinity;
			}else{
				_track._loop_time=_loop_time;
			}
		}else{
			_track._loop_time=_clip.length;
		}
		
		_track.InitTrack();
		
		if(_delay_time<=0){
			_source.Play();
			_track._was_started=true;
			_track.processing += delegate(SoundTrack _self_track){};
		}else{
			_track.startTime+=_delay_time;
			ProcessingSound _delay_start;
			_delay_start = delegate(SoundTrack _self_track){
				#if UNITY_EDITOR
				if(_track==null){
					return;
				}
				#endif
				float _time=Time.realtimeSinceStartup;
				if(_track.startTime<=_time){
					_source.Play();
					if(_track.start_action!=null){
						_track.start_action(_track);
					}
					_track._was_started=true;
					_track.processing += delegate(SoundTrack _self_track_1){};
					_track.processing -= _delay_start;
				}
			};
			_track.processing += _delay_start;
		}
		
		#if UNITY_EDITOR
		if(!UnityEditor.EditorApplication.isPlaying){
			UnityEditor.EditorApplication.CallbackFunction _destroy1 = delegate(){
				_track.destroyTrack(false);
				//Debug.Log("playmodeStateChanged destroy");
			};
			UnityEditor.EditorApplication.CallbackFunction _destroy2 = delegate(){
				if(UnityEditor.EditorApplication.isCompiling){
					_track.destroyTrack(false);
					//Debug.Log("update isCompiling destroy");
				}else if(_track.AudioSrc==null){
					_track.destroyTrack(false);
				}
			};
			UnityEditor.EditorApplication.playmodeStateChanged += _destroy1;
			UnityEditor.EditorApplication.update += _destroy2;
			UnityEditor.EditorApplication.update += _track.Update;
			_track.destroy_action += delegate(SoundTrack _self_track, bool _atEndOfSound, float _time_offset){
				//Debug.Log("destroy_action "+_track.playing_time+" / "+_track.loop_time+" / "+_track.created_time);
				UnityEditor.EditorApplication.playmodeStateChanged -= _destroy1;
				UnityEditor.EditorApplication.update -= _destroy2;
				if(!UnityEditor.EditorApplication.isPlaying){
					UnityEditor.EditorApplication.update -= _track.Update;
				}
				//Debug.Log("destroy_action "+_track);
			};
		}
		#endif
		
		//Debug.Log("PlaySound "+_loop_time+" / "+_delay_time);
		
		return _track;
	}

	bool ownGameobject;
	AudioSource audioSrc;
	public AudioSource AudioSrc{
		get{
			return audioSrc;
		}
	}
	
	public float volume{
		get{
			return audioSrc.volume;
		}
		
		set{
			audioSrc.volume = value;
		}
	}
	
	public float pitch{
		get{
			return audioSrc.pitch;
		}
		
		set{
			audioSrc.pitch = value;
		}
	}
	
	float _last_time_position=0;
	float _time_position=0;
	public float time_position{
		get{
			return _time_position;
		}
	}
	float _playing_time=0;
	public float playing_time{
		get{
			return _playing_time;
		}
	}
	float _life_time=0;
	public float life_time{
		get{
			return _life_time;
		}
	}
	float _loop_time=0;
	public float loop_time{
		get{
			return _loop_time;
		}
	}
	float _delay_time=0;
	public float delay_time{
		get{
			return _delay_time;
		}
	}
	public float created_time{
		get{
			return (startTime-delay_time);
		}
	}
	bool _was_started=false;
	public bool was_started{
		get{
			return _was_started;
		}
	}
	float _length=0;
	public float length{
		get{
			return _length;
		}
	}
	
	float startTime=0;
	void InitTrack () {
		_last_time_position=startTime=Time.realtimeSinceStartup;
		_time_position=0;
		_playing_time=0;
	}
	
	//public float updateRate=0.03f;
	void Update () {
		
		#if UNITY_EDITOR
		if(this==null){
			return;
		}
		#endif
		
		float _delta_time=(Time.realtimeSinceStartup-_last_time_position)*audioSrc.pitch;
		if(_was_started){
			if(audioSrc.isPlaying){
				_playing_time+=_delta_time;
				_time_position+=_delta_time;
				if(_time_position>=_length){
					float _offset=_time_position%_length;
					if(complete_action!=null){
						complete_action(this,_offset);
					}
					_time_position=_offset;
				}
			}else if(!audioSrc.loop&&!AudioListener.pause){
				destroyTrack(true);
				return;
			}
		}
		_life_time+=_delta_time;
		_last_time_position=Time.realtimeSinceStartup;
		
		if(playing_time>=loop_time){
			destroyTrack(true);
			return;
		}
		
		processing(this);
	}

	public void DestroyTrack(){
		destroyTrack(false);
	}

	bool destroyed;
	void destroyTrack(bool atEndOfSound){
		if(destroyed){
			return;
		}
		destroyed = true;
		if(destroy_action!=null){
			destroy_action(this,atEndOfSound,_time_position%_length);
		}
		#if UNITY_EDITOR
		if(!UnityEditor.EditorApplication.isPlaying){
			if(ownGameobject){
				if(audioSrc!=null){
					DestroyImmediate(gameObject,false);
				}
			}else{
				if(audioSrc!=null){
					DestroyImmediate(this,false);
					DestroyImmediate(audioSrc,false);
				}
			}
			return;
		}
		#endif
		if(ownGameobject){
			Destroy(gameObject);
		}else{
			Destroy(this);
			Destroy(audioSrc);
		}
	}
	
	void OnDestroy(){
		destroyTrack(false);
	}
	
	public void setTimePosition(float _time){
		float _diff = _time - _time_position;
		audioSrc.timeSamples=(int)(_time*audioSrc.clip.frequency);
		_time_position+=_diff;
		_playing_time+=_diff;
		if(_time_position>=_length){
			float _offset=_time_position%_length;
			if(complete_action!=null){
				complete_action(this,_offset);
			}
			_time_position=_offset;
		}
	}

	/**Use for looped sound, for check end of sound length.
	 * For NOT looped sound, use 'destroy_action', 
	 * or 'complete_action' and 'destroy_action' for better accuracy'.
	 * NOTE: This action can not be guaranteed when the component is destroyed.*/
	public CompleteSound complete_action=null;
	public ProcessingSound start_action=null;
	public ProcessingSound processing=null;
	/**Use to check the destroying the sound component.
	 * If 'atEndOfSound' is false,  
	 * NOTE: NOT looped sound will be destroyed automatically, when end of sound length.*/
	public DestroySound destroy_action=null;
}

}

