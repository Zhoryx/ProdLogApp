   M S B A M L   `   `   `  ����E  AProdLogApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ProdLogApp.ViewsProdLogApp.Views  5   	   %    ProdLogApp.Views.PromptOperario   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:ProdLogApp.Views     5   	   X TXceed.Wpf.Toolkit, Version=4.7.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  PromptOperario��5	   	   $	��450��6    $	��800��6-   ���#0864AC � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Views/Designs/Style.xaml?�6%   %    5      .��5      � .+�5   
   �� ���#E6F0FA   ShadowBorderStyle#��[  6&   $��10,10,342,10q�6Q    �� Padding$ 10q�6g   ._�5      �� $
��Auto=�6   .��5      �� -   5         ��Name$ Producciones��5      $	��431��61    J� RenderTransformOrigin$ 0.664,0.535&�6n   $	��398��6�    �� View 5      �� 5      .P�5      ��  �� Width$
 Auto��6-   $��Operario��6:   	 �� DisplayMemberBinding	 6L   �� *
Operario+5      5      5       5!      �� 5"   
   
 �� Orientation$
 Vertical=�6   $
��Left=�6,   $��463,158,0,158q�6G   $ 
0.51,0.517&�6^   $	��324��6�   .+�5#      ��   ConfirmButtonStyle#��[ 6   $��	Confirmar��6A   $��10q�6U   �� 5$        CancelButtonStyle#��[ 6   $��Cancelar��6@   5%      �� 5&   
   $��571,46,121,359q�6   $��14(�6(   
���   �$��Buscar Operario��6I   )  #Xceed.Wpf.Toolkit.AutoSelectTextBox  5'   
   -   5          Name$ OperarioAutoComplete��5'   !   $��525,75,75,329q�5(   	   5)      5+      