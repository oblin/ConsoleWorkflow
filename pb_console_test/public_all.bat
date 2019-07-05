if exist %prg_name%\%prg_name%.exe goto run_prg
if not exist %prg_name%.exe goto end 
rem call "C:\Program Files\WinRAR\winrar" x %prg_name%.exe -y -ibck
call "C:\Program Files\WinRAR\winrar" x %prg_name%.exe -y 

:run_prg

set deployDo=N
call d:\00_deploy\D_workspace ..\..\代刚祘Αㄏノ甅ン\now_exe ..\..\代刚祘Αㄏノ甅ン\copy_ini 

if not exist %prg_name%\%prg_name%.exe goto end 
cd %prg_name%
start %prg_name%.exe

:end
echo off
cd ..
del *.lnk
del set_ini.bat
if exist accu.exe        copy ..\..\代刚祘Αㄏノ甅ン\z_accu.bat    
if exist anti.exe        copy ..\..\代刚祘Αㄏノ甅ン\z_anti.bat    
if exist adm.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_adm.bat     
if exist brs.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_brs.bat     
if exist chk.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_chk.bat     
if exist diet.exe        copy ..\..\代刚祘Αㄏノ甅ン\z_diet.bat    
if exist drg.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_drg.bat     
if exist drug.exe        copy ..\..\代刚祘Αㄏノ甅ン\z_drug.bat    
if exist ecl.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_ecl.bat     
if exist emp.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_emp.bat     
if exist ins.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_ins.bat     
if exist insn.exe        copy ..\..\代刚祘Αㄏノ甅ン\z_insn.bat    
if exist ipd.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_ipd.bat     
if exist ipd_order.exe   copy ..\..\代刚祘Αㄏノ甅ン\z_ipd_order.bat     
if exist lab.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_lab.bat     
if exist mib.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_mib.bat     
if exist min.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_min.bat     
if exist mtn.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_mtn.bat     
if exist opd.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_opd.bat     
if exist opda.exe        copy ..\..\代刚祘Αㄏノ甅ン\z_opda.bat    
if exist opdn.exe        copy ..\..\代刚祘Αㄏノ甅ン\z_opdn.bat    
if exist opdr.exe        copy ..\..\代刚祘Αㄏノ甅ン\z_opdr.bat    
if exist oreg.exe        copy ..\..\代刚祘Αㄏノ甅ン\z_oreg.bat    
if exist org.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_org.bat     
if exist ppf.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_ppf.bat     
if exist priv.exe        copy ..\..\代刚祘Αㄏノ甅ン\z_priv.bat    
if exist rep.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_rep.bat     
if exist ris.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_ris.bat     
if exist sal.exe         copy ..\..\代刚祘Αㄏノ甅ン\z_sal.bat     
if exist stock.exe       copy ..\..\代刚祘Αㄏノ甅ン\z_stock.bat    
