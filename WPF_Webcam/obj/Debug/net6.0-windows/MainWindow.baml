   M S B A M L   `   `   `  ����E  AWPF_Webcam, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:WPF_Webcam
WPF_Webcam  5   	       WPF_Webcam.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:WPF_Webcam     5   	     � Title$  
MainWindow��5   	   $	��469��6   $	��946��6)   .��5	      � $��0,0,-17,-16q�6   .+�5
   
   �� -   5         ��Name$ 	txtBrowes��5
      $
��Left=�6%   $��45��6@   $��	23,61,0,0q�6L    �� TextWrapping$
 Wrap=�6_   $	��Top=�6s   $	��371��6�   �� 5   
   -   5         ��Name$ 	btnBrowes��5      $��Browes��6$   $
��Left=�65   $��45��6P   $��
432,61,0,0q�6\   $	��Top=�6p   $��73��6�   6�   �� 5   
   $��Webcam��6   $
��Left=�6!   $��45��6<   $��
152,16,0,0q�6H   $	��Top=�6\   $	��114��6t    � 5   
   -   5          �Name$ frameCamera��5      $��Camera��6%   $
��Left=�66   $	��214��6Q   $��
596,26,0,0q�6^   $	��Top=�6r   $	��295��6�   6�   �� 5   
   -   5        $ 
btnCapture��5      $��Capture��6%   $
��Left=�67   $��40��6R   $��700,373,0,0q�6^   $	��Top=�6s   $��88��6�   6�   �� 5   
   -   5        $ 	btnDelete��5      $��Delete��6$   $
��Left=�65   $��40��6P   $��820,373,0,0q�6\   $	��Top=�6q   $��88��6�   6�   �� 5   
   -   5        $��Choose Image��5      $
��Left=�6(   $��40��6C   $��536,373,0,0q�6O   $	��Top=�6d   $	��120��6|   6�   �� 5   
   -   5        $��Upload��5      $
��Left=�6"   $��40��6=   $��536,310,0,0q�6I   $	��Top=�6^   $	��120��6v   6�   �� 5   
   -   5        $��Resize Image��5      $
��Left=�6(   $��Bottom=�6C   $��820,0,0,119q�6^   $��40��6�   $��88��6�   �� 5   
   -	   5         ��Name$ checkBoxCamera��5      $��Bật/Tắt Camera��6+   $��542,255,298,178q�6~   �� 5   
   -
   5         ��Name$ image��5      $
��Left=�6   $��97��6:   $��682,255,0,0q�6F   $	��Top=�6[   $	��101��6s   �� 5   
   -   5         ��Name$ 	listView1��5      $
��Left=�6&   $	��260��6A   $��
23,153,0,0q�6N   $	��Top=�6b   $	��482��6z    �� View 5      �� 5      .P�5      �� $��Image��6%   	 �� Width$		 100��64   
 �� CellTemplate
 5      �� 5      .R�5   "   �� $��50��6G   $��50��6R   ��6(   �� *ImageSource+6R   5      5       �� 5!      $��	File Name��6%   $		 380��68   
 5"      �� 5#      .R�5$   "   �� $��Center=�6F   ��6,   �� *
FileName+6F   5%      5'      5(      5*      5.      5/      