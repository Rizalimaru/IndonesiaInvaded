# Code Convention

- Format Penamaan File
    - Format Kapital (Contoh: M_AWAN)
        - Material: M_(nama material)
        - Model: MD_(nama model)
        - Props: P_(nama props)
        - Texture: T_(nama texture)
        - VFX: VFX_(nama vfx)
        - SFX: SFX_(nama sfx)
        - BGM: BGM_(nama bgm)
        - Aset UI: UI_(nama aset)
    - Format Camel Case (Contoh: characterController)
        - Scripts: namaScript
        - Scene: namaScene
- Struktur Folder
    
    Berikut Format Stuktur Folder :
    
    - Assets
        - Scripts
            
            
        - Materials
        - Audio
            - Bgm
            - Music
            - SFX
        - Art
            - Model
            - Environtment
            - Textures
            - Animation
            - UI
        - Level
            - Prefabs
            - Scene
      
        - Asbres // Asset Bundle Resources
            - Character
                - Avatar
                    - (AvatarName)
                        - Avatar_(00)
                            - {Art_(AvatarName)_(00).fbx} 
                            - Animation 
                				- {Avatar_(AvatarName)_(00)_ani_?-.fbx}
                            - Texture
                				- {(AvatarName)_(00)_?-.tga/.exr/.png}
                            - Material
                				- {AvatarName)_(00)_mat_?-.mat)
                - NPC // wip
                    - 
                - Monster
                    - (MonsterName)_(00)
                        - {Art_(MonsterName)_(00).fbx}
                        - Animation
                            - {Monster_(MonsterName)_(00)_ani_?-.fbx}
                        - Texture
                            - {(MonsterName)_(00)_?-.tga/.exr/.png}
                        - Material
                            - {MonsterName)_(00)_mat_?-.mat)
                - CharacterPrefabs
                    - Player
                    - Monster
                    - NPCMonster
                    - NPC
                - Emotion
                - EmotionClip
            - UI
		- 
	    - Effect
                - Eff_Model
                    - Avatar / Monster / Scene / Common
                        - (AvatarName)_(00) / (Monster)_(00) / etc
                            - {Eff_Avatar_(AvatarName)_(00)_?_?.fbx}
                - Eff_Texture
                    - Special / Streak / Trail / Splash / Symbol / Aura / Smoke / Line // each or(/) is independent folder
                    - (AvatarName)
                        - Avatar_(00)
                            - {Eff_Avatar_(AvatarName)_(00)_?_?.fbx}
                - Eff_Material
		    - 
                - Eff_Animation
		    - 
                - Eff_Prefab
		    - 

- Standar Coding
