if exist %prg_name%\%prg_name%.exe goto run_prg
if not exist %prg_name%.exe goto end 
rem call "C:\Program Files\WinRAR\winrar" x %prg_name%.exe -y -ibck
call "C:\Program Files\WinRAR\winrar" x %prg_name%.exe -y 

:run_prg

set deployDo=N
call d:\00_deploy\D_workspace ..\..\���յ{���ϥήM��\now_exe ..\..\���յ{���ϥήM��\copy_ini 

if not exist %prg_name%\%prg_name%.exe goto end 
cd %prg_name%
start %prg_name%.exe

:end
echo off
cd ..
del *.lnk
del set_ini.bat
if exist accu.exe        copy ..\..\���յ{���ϥήM��\z_accu.bat    
if exist anti.exe        copy ..\..\���յ{���ϥήM��\z_anti.bat    
if exist adm.exe         copy ..\..\���յ{���ϥήM��\z_adm.bat     
if exist brs.exe         copy ..\..\���յ{���ϥήM��\z_brs.bat     
if exist chk.exe         copy ..\..\���յ{���ϥήM��\z_chk.bat     
if exist diet.exe        copy ..\..\���յ{���ϥήM��\z_diet.bat    
if exist drg.exe         copy ..\..\���յ{���ϥήM��\z_drg.bat     
if exist drug.exe        copy ..\..\���յ{���ϥήM��\z_drug.bat    
if exist ecl.exe         copy ..\..\���յ{���ϥήM��\z_ecl.bat     
if exist emp.exe         copy ..\..\���յ{���ϥήM��\z_emp.bat     
if exist ins.exe         copy ..\..\���յ{���ϥήM��\z_ins.bat     
if exist insn.exe        copy ..\..\���յ{���ϥήM��\z_insn.bat    
if exist ipd.exe         copy ..\..\���յ{���ϥήM��\z_ipd.bat     
if exist ipd_order.exe   copy ..\..\���յ{���ϥήM��\z_ipd_order.bat     
if exist lab.exe         copy ..\..\���յ{���ϥήM��\z_lab.bat     
if exist mib.exe         copy ..\..\���յ{���ϥήM��\z_mib.bat     
if exist min.exe         copy ..\..\���յ{���ϥήM��\z_min.bat     
if exist mtn.exe         copy ..\..\���յ{���ϥήM��\z_mtn.bat     
if exist opd.exe         copy ..\..\���յ{���ϥήM��\z_opd.bat     
if exist opda.exe        copy ..\..\���յ{���ϥήM��\z_opda.bat    
if exist opdn.exe        copy ..\..\���յ{���ϥήM��\z_opdn.bat    
if exist opdr.exe        copy ..\..\���յ{���ϥήM��\z_opdr.bat    
if exist oreg.exe        copy ..\..\���յ{���ϥήM��\z_oreg.bat    
if exist org.exe         copy ..\..\���յ{���ϥήM��\z_org.bat     
if exist ppf.exe         copy ..\..\���յ{���ϥήM��\z_ppf.bat     
if exist priv.exe        copy ..\..\���յ{���ϥήM��\z_priv.bat    
if exist rep.exe         copy ..\..\���յ{���ϥήM��\z_rep.bat     
if exist ris.exe         copy ..\..\���յ{���ϥήM��\z_ris.bat     
if exist sal.exe         copy ..\..\���յ{���ϥήM��\z_sal.bat     
if exist stock.exe       copy ..\..\���յ{���ϥήM��\z_stock.bat    
