using System;
using System.IO;
using Xunit;

namespace BlurHashSharp.Drawing.Tests
{
    public class BlurHashEncoderTests
    {
        [Theory]
        /* These don't exist anymore on samples.ffmpeg.org
        [InlineData("samples.ffmpeg.org/icons/text.gif", 4, 4, "UQM@=i^6PA-B.8rrWVM{NujFkCIU_NaKoft7")]
        [InlineData("samples.ffmpeg.org/icons/blank.gif", 4, 4, "UGNfG1?^fQ?^?vkCfQkCfQfQfQfQ?vkCfQkC")]
        [InlineData("samples.ffmpeg.org/icons/movie.gif", 4, 4, "UJGvFjD4TxU^tRRPj[oLD%Rj%Moft7RPofoL")]
        [InlineData("samples.ffmpeg.org/icons/unknown.gif", 4, 4, "UcL;]T,@%#-pS2aKxuRjNaM{RjNG~qVsM{%M")]
        [InlineData("samples.ffmpeg.org/icons/image2.gif", 4, 4, "UaLXb]|s.8NL^jiJM|bv%NR,J6NFp_pHxtVu")]
        [InlineData("samples.ffmpeg.org/icons/hand.right.gif", 4, 4, "UyNd:Y#QXnpIt7ozWBjYUHOsnhtQyDVsofX8")]
        [InlineData("samples.ffmpeg.org/icons/compressed.gif", 4, 4, "UTNwi;=KOrQ-yrWBxut,8vNa%~ui^Qi_H?n4")]
        [InlineData("samples.ffmpeg.org/icons/back.gif", 4, 4, "UmH:%H.S%#ozyD%gtRRP?^Mxozj[MdMxtRof")]
        [InlineData("samples.ffmpeg.org/icons/folder.gif", 4, 4, "U#N,Yi-UEj%2*JRjoJWqC8R-%1X9.8ozRjoe")]
        */
        [InlineData("samples.ffmpeg.org/image-samples/personal_splash.png", 4, 4, "UgIE9-WnImRP01xaWBkBtfM{xbofR7X4bae:")]
        [InlineData("samples.ffmpeg.org/image-samples/delete_node_large.png", 4, 4, "UCL;mj%N?W~p?axsWEM}-,t6IbRn?bWEoct6")]
        [InlineData("samples.ffmpeg.org/image-samples/RedbrushAlpha.png", 4, 4, "UIAR-S+#B6AVwKfjJ8a{0}K0#.,@K3a#$jsV")]
        [InlineData("samples.ffmpeg.org/image-samples/digital-applications.png", 4, 4, "UORVqmDPaxMzn,ayofRj?]kDx[tPxuofV[oy")]
        [InlineData("samples.ffmpeg.org/image-samples/ball.png", 4, 4, "U#M+8hyB}_-XyBkBrwjI~ErxIno{,_jIo{oy")]
        [InlineData("samples.ffmpeg.org/image-samples/add_node_small.png", 4, 4, "UEKx6z%N?W~ot9-.t7D+%F-:N3D+?bj]oft6")]
        [InlineData("samples.ffmpeg.org/image-samples/delete_node_small.png", 4, 4, "UIL4y*t9%F~o%Nxsa#Rk%J%LN1M}-;a#j@t6")]
        [InlineData("samples.ffmpeg.org/image-samples/free_splash.png", 4, 4, "UgIEC[WnImRP01xaWBkBtfM{xbofR7X3kBe:")]
        [InlineData("samples.ffmpeg.org/image-samples/change_label_large.png", 4, 4, "UBMQ*N%NW7_2~pxtWFM}RhWF-;xs-;RjfPog")]
        [InlineData("samples.ffmpeg.org/image-samples/open_large.png", 4, 4, "UIJkl}?ct0a%~m?ZN2ogoaD*oefSxwt2t1WC")]
        [InlineData("samples.ffmpeg.org/image-samples/right_triangle.png", 4, 4, "UMC*tR??MkRYbLWEodkB8-RU%ZtNx]a#V^kB")]
        [InlineData("samples.ffmpeg.org/image-samples/progress.png", 4, 4, "UREC3ZtI?J%4D#oPM_s;4,IUImM{X3xuogRi")]
        [InlineData("samples.ffmpeg.org/image-samples/delete_member_small.png", 4, 4, "UMKBRS%Mt7N3?ZxsRkt5WEa$j=%K~o%LWFa#")]
        [InlineData("samples.ffmpeg.org/image-samples/money-256.png", 4, 4, "UcP6:N^+oLIU8^V?j]oy?HD$WCkD-;RjWBR+")]
        [InlineData("samples.ffmpeg.org/image-samples/IceAlpha.png", 4, 4, "UsC[ndtTXAaxDgbbo#adoLadbckCaxkDaekE")]
        [InlineData("samples.ffmpeg.org/image-samples/cool.bmp", 4, 4, "UiH^#oR%E0s;~ER%EJa{-Xf7NFazxbR%s:of")]
        [InlineData("samples.ffmpeg.org/image-samples/delete_method_large.png", 4, 4, "UELXV;?aW9-;_0%KRpoet1WFxuRk~qWEIUfR")]
        [InlineData("samples.ffmpeg.org/image-samples/money-2.bmp", 4, 4, "UT6a:iomawatW5fSfOfSokW6fSj^j|avfQfO")]
        [InlineData("samples.ffmpeg.org/image-samples/test.png", 4, 4, "UVRysgof%Mayayayj[j[~qfQIUofxuofWBWB")]
        [InlineData("samples.ffmpeg.org/image-samples/screen.png", 4, 4, "UTPQ8B4o4o00ohRkozt7E3axRjxuN1WAoeof")]
        [InlineData("samples.ffmpeg.org/image-samples/money-256.bmp", 4, 4, "UcP6:N^+oLIU8^V?j]oy?HD$WCkD-;RjWBR+")]
        [InlineData("samples.ffmpeg.org/image-samples/component.png", 4, 4, "UcNQ?E$m[4=P{FKHKHs;{FnmNrr^[NwikCnm")]
        [InlineData("samples.ffmpeg.org/image-samples/money.jpg", 4, 4, "UcP6:N^+j[IU8^V?j]oz?HD$WCkD-;RjWBWC")]
        [InlineData("samples.ffmpeg.org/image-samples/invalid_class.png", 4, 4, "UeNc${%2?]-=~oRkIVj]_MoyVXsl_2xtspt6")]
        [InlineData("samples.ffmpeg.org/image-samples/mail.png", 4, 4, "UeO43P%MpL-=~DozRnaeEmkDVqV?x_V@oatR")]
        [InlineData("samples.ffmpeg.org/image-samples/14121200.jpg", 4, 4, "UTHLSTwb-;IT?dRPt8IUsiNGs:Rj9bM{xbof")]
        [InlineData("samples.ffmpeg.org/image-samples/methods_large.png", 4, 4, "UHKUZ#-;avj_~nt7N0of%DIW-:xt%MRjofWD")]
        [InlineData("samples.ffmpeg.org/image-samples/add_component_large.png", 4, 4, "UCKUZw-;~n%M~oxtD+Rj_0xtIWt6?aj]ogt7")]
        [InlineData("samples.ffmpeg.org/image-samples/delete_component_small.png", 4, 4, "UEIrEhN3^~-=~oxs9HRj~n-:IVxs~oxuogt7")]
        [InlineData("samples.ffmpeg.org/image-samples/money-256-rle.bmp", 4, 4, "UcP6:N^+oLIU8^V?j]oy?HD$WCkD-;RjWBR+")]
        [InlineData("samples.ffmpeg.org/image-samples/dither.jpg", 4, 4, "U@KcPS}Uq]n4VonnogkCIcWTt6oexooMV]WC")]
        [InlineData("samples.ffmpeg.org/image-samples/add_method_large.png", 4, 4, "UELN.F?bav-;_0%LWGWBoZWFxuM|~qa#IUj[")]
        [InlineData("samples.ffmpeg.org/image-samples/opened_node.png", 4, 4, "UmQ;JC$m+k,LkWbFogbG:bnmKabF{rsDR%r_")]
        [InlineData("samples.ffmpeg.org/image-samples/forsaken.jpg", 4, 4, "UCCYq6#79FI:00tRt7oyDiNZ?bog-=WB55NG")]
        [InlineData("samples.ffmpeg.org/image-samples/sspline.BMP", 4, 4, "ULNv*+ystTJ-R8M{kVxapyQ,ROogF2xDi_Rk")]
        [InlineData("samples.ffmpeg.org/image-samples/delete_component_large.png", 4, 4, "UDKd}W%M~m-;~ot6D+WB_0xtIWxt-;fSogt7")]
        [InlineData("samples.ffmpeg.org/image-samples/logo.bmp", 4, 4, "UkTP*dmlh0mRujeTgheTgNe.e.gNwxeTf+g3")]
        [InlineData("samples.ffmpeg.org/image-samples/authentica.jpg", 4, 4, "UJ9@S5M{WB%MRjj[WBj[00WBofj[xufQofj[")]
        [InlineData("samples.ffmpeg.org/image-samples/off_l16.bmp", 4, 4, "UK6w=zm2i*gGl5iMe+gFakcDf9e=a,b_g3i_")]
        [InlineData("samples.ffmpeg.org/image-samples/fujifilm-finepix40i.jpg", 4, 4, "ULF~BYR5oa01s:%24.kC9ZNHNHx]MxRjx]bI")]
        [InlineData("samples.ffmpeg.org/image-samples/magick.png", 4, 4, "UPRC-@~q-;9F-oD%kD%M%3oJD%xv%NM{t7t7")]
        [InlineData("samples.ffmpeg.org/image-samples/money-24bit.png", 4, 4, "UcP6:N^+oLIU8^V?j]oy?HD$WCkD-;RjWBR+")]
        [InlineData("samples.ffmpeg.org/image-samples/change_label_small.png", 4, 4, "UJLXV:xvW8_2~p%KWFWFj;og-:WB%MWBxtRk")]
        [InlineData("samples.ffmpeg.org/image-samples/logo.png", 4, 4, "UPDHzGs:5Onnsqj@WUa#0xag=#k9t6azj[ju")]
        [InlineData("samples.ffmpeg.org/image-samples/run_large.png", 4, 4, "UcM$[P-6e9-8*{ayofayhgWCt6Rk[Uj@W=of")]
        [InlineData("samples.ffmpeg.org/image-samples/run_small.png", 4, 4, "ULJb2M%ND,-;t7WBocj?02RjxrRi-,j?RlWA")]
        [InlineData("samples.ffmpeg.org/image-samples/corbis.png", 4, 4, "UISFz|rBv|xu~pRjRjax?vT1SiWBWX%Lt7WB")]
        [InlineData("samples.ffmpeg.org/image-samples/delete_member_large.png", 4, 4, "UHLXV;-;fMog?Zt6Rpt7Rij[xtt7~pt8M{fO")]
        [InlineData("samples.ffmpeg.org/image-samples/right_triangle_option.png", 4, 4, "UHGTNmNN%M_N?vt7WBfk_2x@IURR%#ogt7kB")]
        [InlineData("samples.ffmpeg.org/image-samples/humdizzle.png", 4, 4, "U24VO5.l$#L~vL#.bwpb;pieNaX+PTO?R5H?")]
        [InlineData("samples.ffmpeg.org/image-samples/appligent.jpg", 4, 4, "UKRD7jNZxuxu~WRjNGs;~DRjRjay4.xaxaa{")]
        [InlineData("samples.ffmpeg.org/image-samples/save_large.png", 4, 4, "UTI=M:oeIWj^~pt7D%M_RjWB%Kt7D$Rj?axu")]
        [InlineData("samples.ffmpeg.org/image-samples/examples.jpg", 4, 4, "UJN^h]IUxaVtoetRx]of~qRQbbko%2xujZRj")]
        [InlineData("samples.ffmpeg.org/image-samples/open_small.png", 4, 4, "UKIrEg_2j;M~~n-:RpWExtM|xtt8xwoft2oc")]
        [InlineData("samples.ffmpeg.org/image-samples/earth2.jpg", 4, 4, "UVE3I:M~IVIp0eNI%1xt=}t5NGax57t3Woog")]
        [InlineData("samples.ffmpeg.org/image-samples/home.png", 4, 4, "UoMaRyxuTd-p~En+W;ozX.kCn4f8O;ofV?V@")]
        [InlineData("samples.ffmpeg.org/image-samples/convert.png", 4, 4, "UOHLl6RkRjj[~qj[M{ay_2RkWBayt7azt7WB")]
        [InlineData("samples.ffmpeg.org/image-samples/add_member_small.png", 4, 4, "UKJ[I|-;t7D;-.%LRlawM|ogobxs~o%LWFWC")]
        [InlineData("samples.ffmpeg.org/image-samples/magick_small.png", 4, 4, "UWQvn3~px]D*-oD%ozxuxbs.D*xu%gM{xut7")]
        [InlineData("samples.ffmpeg.org/image-samples/add_method_small.png", 4, 4, "UKJ*uQ?bM~j_?Z-:a$j=WAofxtax~pxtIWRk")]
        [InlineData("samples.ffmpeg.org/image-samples/ellipsis.png", 4, 4, "UTL;me-;fQ-;~qt7fQt7RjayfQay%Mj[fQj[")]
        [InlineData("samples.ffmpeg.org/image-samples/original.png", 4, 4, "U~SWcEnighnixGj[a|jtdBbbnObbW;fQjtfQ")]
        [InlineData("samples.ffmpeg.org/image-samples/methods_small.png", 4, 4, "UIJRdr?aN0%N~nj^M}WCRifS?YxsxuRit7oe")]
        [InlineData("samples.ffmpeg.org/image-samples/TESTcmyk.jpg", 4, 4, "UuQ6|AR,}Q$##7oeNER*j;jsXAWr,nWXOGs,")]
        [InlineData("samples.ffmpeg.org/image-samples/sony-cybershot.jpg", 4, 4, "UhFiPyaeM_Rk_4V[WBof-;S2t7of%MbIWVWB")]
        [InlineData("samples.ffmpeg.org/image-samples/money-16.bmp", 4, 4, "UbP6:N?bflIU4mRiflt7?bD$WBog-;RjWBWB")]
        [InlineData("samples.ffmpeg.org/image-samples/add_component_small.png", 4, 4, "UBIY6DWI~m%O~o-.4qIU~m_1IVoa~p-:t8a#")]
        [InlineData("samples.ffmpeg.org/image-samples/add_node_large.png", 4, 4, "UBL#2=-=?X~o-;%KWFIW-+xtIbRl_3Rloct7")]
        [InlineData("samples.ffmpeg.org/image-samples/make_small.png", 4, 4, "UIKni4-:~n?c-;j[t7of~nt7D+aw?cj[WBof")]
        [InlineData("samples.ffmpeg.org/image-samples/delete_method_small.png", 4, 4, "UMK1%v-;M}t9_1-.a$t6obayxsoe~pt7IWWD")]
        [InlineData("samples.ffmpeg.org/image-samples/save_small.png", 4, 4, "UIG+Umt34qM|~pt74o9F%MogW9Ri00IU-;%M")]
        [InlineData("samples.ffmpeg.org/image-samples/TEST24rle.BMP", 4, 4, "UqFYHsjXKBXBI:j]M_adR%a#xDoINhjYs%f,")]
        [InlineData("samples.ffmpeg.org/image-samples/pngalpha.png", 4, 4, "UWP?w8?b%MM{4mi_tRtR~WDioztR^+M{bIW=")]
        [InlineData("samples.ffmpeg.org/image-samples/TESTbw_os2.bmp", 4, 4, "U0SWr+}]fQ}]}]fQfQfQfQfQfQfQ}]fQfQfQ")]
        [InlineData("samples.ffmpeg.org/image-samples/deliverance800.jpg", 4, 4, "UB8=NK%iV].9oz%N%Nt7x^xvt7og?wogWAxv")]
        [InlineData("samples.ffmpeg.org/image-samples/pngwriter-problem.png", 4, 4, "U80M3~h0a0Y+j[dWbHb^ZNeTkqg3Z$g$kCjF")]
        [InlineData("samples.ffmpeg.org/image-samples/professional_splash.png", 4, 4, "UgIE6yWUImRj0KxbWBkBtfM{xboeR7X3kCay")]
        [InlineData("samples.ffmpeg.org/image-samples/kodak-dc210.jpg", 4, 4, "UA9GaZpctQ.70ez;IA9Zn~O?=|-pWAnPNbS0")]
        [InlineData("samples.ffmpeg.org/image-samples/add_member_large.png", 4, 4, "UHLN.F-;oZRl?YxsRpofRioet7t7~pxuRjoc")]
        [InlineData("samples.ffmpeg.org/image-samples/make_large.png", 4, 4, "UFL4y$%M~n?b?aj[WEj[~ot6D*WC-;j[j@of")]
        [InlineData("samples.ffmpeg.org/image-samples/11Bos20.bmp", 4, 4, "UXD9^,%iM{Ri0VS9xuocx{a%ROWARPn+s+ax")]
        [InlineData("samples.ffmpeg.org/image-samples/closed_node.png", 4, 4, "UnQ;bsTb+R{r,Kf6fkoM+Rr_KHS0,xjbjZoM")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/artists.gif", 4, 4, "UG9Zs2R*4.t7?HWVIUof0foL%2ay0eoM-pWB")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/4_directions.gif", 4, 4, "UG9tIut409IZt4j@WDaz09WD-*j@IZazj@WD")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/top_title_green_frog.gif", 4, 4, "ULM*Ba~S4?^%~VNKxsR+D$M}xsIVD$IVxtWB")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/creatures_001.gif", 4, 4, "UA5=gf%b8*91xqt4RmM$s,n*WXbZy8kBR9V]")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/Alien-blue.gif", 4, 4, "UlPA%4bc{c+u,.sSnini;0WWOZS$nnShS#W=")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/smiley_010.gif", 4, 4, "UoQ0TtV;%QtAt1oeRkRjxzNGxsxuxwRjozkD")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/3d_book2.gif", 4, 4, "UYC?r^-;9FIUofRjM{t700IU%MxuWBxuxuRj")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/2_computers.gif", 4, 4, "U78z.9bF~ot6-?ayRkazIRayM^WU-waxxnj=")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/atom1.gif", 4, 4, "UXO.I0%248tlTxX8r?niGtay#8kC-BoLWVjZ")]
        //[InlineData("samples.ffmpeg.org/image-samples/GIF/bee.gif", 4, 4, "UwEVs=ayozog~oayozof?Zayogofxtafofj[")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/free-gifs.gif", 4, 4, "UfGR;}~V%LbJ?boeoLof%LRjWBay-=j]j@ay")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/houses_002.gif", 4, 4, "UcM@7ixuTc?a$3oekrR+TJofwKWB.mWXZ#xZ")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/1.gif", 4, 4, "U~DKySj[fPj[?wj[f6j[-ffPfQfP$|fQfQfQ")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/Butterfly.gif", 4, 4, "URB3Qljs0Kt7o0s:ofRj4.of?HRjR+NGWAxu")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/7up.gif", 4, 4, "UwPjDU-;~qIUx]ofM{Rj-;RjIUtRxaRjoft7")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/2c-button_green_blue2.gif", 4, 4, "U31NdMVAQSpQQUp8Z}f3aJfgk8aepwZ@kYfS")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/Angry_barbarian.gif", 4, 4, "UE8fo;Na0|$iJToKwvWX0|xG={Nb;fS2K5sm")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/8ball.gif", 4, 4, "UJAc#lt700M{EjfRWBWV4TWB_3xuRjayxuof")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/Accident.gif", 4, 4, "UFSs50D%IUM{~qj[ayf7%M%Mxuxut7WBWBay")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/truckbut.gif", 4, 4, "UA3w59baD-j@Dnf7%Ia|upe:NIfjYKf*oejZ")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/Astronaut1.gif", 4, 4, "UAB3._IU9F-;WBIUIUxu00t7tRRj8_xuxuRj")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/3D.gif", 4, 4, "UB6i:OR*1a$kS1oLspS11asp]:NZ$kS1NZ$j")]
        // [InlineData("samples.ffmpeg.org/image-samples/GIF/love_001.gif", 4, 4, "UWFzBuWX5rxD}bfRWZjsvxoJs+WX$Mf6R*bH")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/2c_button-yellow_red.gif", 4, 4, "UDBeNgtg1Li}AFaNI?WDR,WWn*WW=uowxXfj")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/red_Dot.gif", 4, 4, "UwRxr^%2*J?bv#WBtlt7yqogm,jY?vt7nig3")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/bettie-gifs-3.gif", 4, 4, "UWI;YV4.0KS2E2oft7ae%LaeaeozE1WV%2j[")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/2_workers.gif", 4, 4, "U87wg4^ks:Rj4maxSPSi.8M{RkWV4on,xCs+")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/bank.gif", 4, 4, "UzPP+Qjt_Nay%gj[jZayofofayaeM{WAayoz")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/Baby_and_stork.gif", 4, 4, "UeOWmJ_1D%_4~pjqM{t8E1RjM{RP?cxvRioI")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/Big_counter.gif", 4, 4, "UFECwdRjM{t7~qayM{of?bj[%Mj[WBay%Mof")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/cliff-gifs-trc-32c.gif", 4, 4, "UrKuXW}Ya0XAxxoNn%f5jDWAW=fRohj]jYju")]
        [InlineData("samples.ffmpeg.org/image-samples/GIF/4_rhombuses.gif", 4, 4, "U~N+*ZvoeutkvuC4OW#FewOXW+nitb#BniX4")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/test16bf555.bmp", 4, 4, "UJGIyDOkIl:k4TEVGZT*tloui_V}nOnlj?oa")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/themole.bmp", 4, 4, "UAF41e5k{vxaBOjF$lbHE1ae-:jFKNWBwNoe")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/test1.bmp", 4, 4, "U47Y|Hipebi:p3eJeJeJebd+ebe1p2eJeJeJ")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/8bpp-rle.bmp", 4, 4, "U64.9MI$V%i.RstKn#rqM;s$XKo#o7r?WHOs")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/4bpp-rle.bmp", 4, 4, "U45.~5YF4EUAX|RVt8XfG~v.+QS[c,obRPnp")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/rmoney.bmp", 4, 4, "UcP6,G^+oLIU4mV?j]oz?HD$WCkD-;RjWBR+")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/money-16-rle.bmp", 4, 4, "UbP6:N?bflIU4mRiflt7?bD$WBog-;RjWBWB")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/testcompress8.bmp", 4, 4, "UUCQ3=VU|_U;Un^QW~%#x]WTs.V|WGofX4bv")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/test32.bmp", 4, 4, "UMGSM;ORNC+I_N$[+Hq.tlt2nOaRtRX5ayWG")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/test16.bmp", 4, 4, "UJGIyDTwEb-$4TIeB;Pbtlt2nhnBr?ain+k8")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/test32v5.bmp", 4, 4, "UKEox[nfM_s6RIR%V=W=qsNMJ6kGA+xaXV$L")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/test4os2v2.bmp", 4, 4, "U4D8Rp~C^k-V^QxGxGR*5QxZs:s:N_bHj[R*")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/test16bf565.bmp", 4, 4, "UJGI$KORIl:k4TEVGZT*tloui_V}nOnljsoa")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/test24.bmp", 4, 4, "UMGSM;ORNC+I_N$[+Hq.tlt2nOaRtRX5ayWG")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/test4.bmp", 4, 4, "UdE3PEr9{zh[Um^QW$%#x]R%s:WFW=baW.X8")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/test8.bmp", 4, 4, "UUCQ3=VU|_U;Un^QW~%#x]WTs.V|WGofX4bv")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/testcompress4.bmp", 4, 4, "UdE3PEr9{zh[Um^QW$%#x]R%s:WFW=baW.X8")]
        [InlineData("samples.ffmpeg.org/image-samples/bmp-files/11Bbos20.bmp", 4, 4, "U1SWr+-pfQ-p^kfQfQfQfQfQfQfQ^kfQfQfQ")]
        [InlineData("samples.ffmpeg.org/image-samples/ART/6-BOATS.png", 4, 4, "UA8;Z6?wp0o#9GIptRxvxt-;j?WXtSWAj@ae")]
        [InlineData("samples.ffmpeg.org/image-samples/ART/8257_p_t_477x480_image02.png", 4, 4, "UOJtSIN0Ipi]~q%Lad9FI[xtkBMx%MM{t7xZ")]
        [InlineData("samples.ffmpeg.org/image-samples/ART/8266_p_t_640x480_image01.png", 4, 4, "UPF~:fI801Ri4mxv-;kC00WYs+ozMwayRloe")]
        [InlineData("samples.ffmpeg.org/image-samples/ART/8257_p_t_640x480_image01.png", 4, 4, "UaDJkzD%R+axtSoyWBay00xut6j]M{M_t7kC")]

        public void Encode_Success(string relPath, int xComponent, int yComponent, string expectedResult)
        {
            string absPath = Path.Join(Environment.GetEnvironmentVariable("FFMPEG_SAMPLES_PATH"), relPath);
            Assert.Equal(expectedResult, BlurHashEncoder.Encode(xComponent, yComponent, absPath));
        }
    }
}
