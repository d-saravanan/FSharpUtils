makecert.exe -n "CN=CelloSaaS_SSC" -r -pe -a SHA1 -len 4096 -cy authority -sv CelloSaaS.pvk CelloSaaS.cer


pvk2pfx.exe -pvk CelloSaaS.pvk -spc CelloSaaS.cer -pfx CelloSaaS.pfx -po 67280421310721