# AES128WindowsFolderEncryption


# A little GUI Encryption Tool for Windows 

I was bored so i tried to learn cryptographic development on windows.
As a forensic analyst, u sometime have to work with sensitive data and its better to encrypt them for the transfer / copy / storage or whatever.



## Disclamer 

The tool is still in BETA and will crash if you don't provide it the right files it ask.


A compiled version is available one the release section.

You can still compile it yourself with MSbuild : 

```bash
MSBuild.exe /t:Publish /p:SelfContained=True /p:Configuration=Release /p:Plateform=x86 /p:PublishDir=C:\Users\WHEREVERUWANTTOPUBLISHIT
```


## How to 

So to encrypt large amount of data, symmetrical algorithm are way more efficient than asymmetrical.
So i decide to use AES128 to encrypt the files.

Thing is, we need to pass the key between two people in a secure way. That's where asymmetrical cryptographic come in place. For this one, i used RSA.

So basically, data are encrypted with AES and a random generated Key. Then this key is encrypted using a public RSA Keys. 

The tool is pretty straight forward : 
- One button to create RSA keys;
- One button to encrypt a folder;
- One button to decrypt an AES container.

The tool will ask you the location of what it needs, you just have to click.
Keep in mind that's a beta, it will crash if you provide it the wrong files, like a pub key instead of a private.

It will also crash if u give to him the wrong keys.

## Info 

While encrypting the tool create 2 files :

- an txt file containing the cyphered AES key (name is > nameofthefolder_CypheredRSAKey.txt )
- an encrypted file with .aes extension (name is > nameofthefolde.zip.aes )


While decrypting it create a folder with the same name as the one chosen to be encrypted.

Be sure not to have already a folder containing the same name in the same directory or it will probably crash.

(ex if your container name is HTB.zip.aes, don't decrypt it in a directory containing a folder name HTB).


Tadaaa.

Hope it helps.