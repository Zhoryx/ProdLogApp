   M S B A M L   `   `   `  ����E  AProdLogApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ProdLogApp.ViewsProdLogApp.Views  5   	   )    #ProdLogApp.Views.PositionManagement   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:ProdLogApp.Views     5   	     � Title$  Menu - Producciones��5   	   $	��720��5	   	   $
��1080��5
   	    � WindowStartupLocation$ CenterScreen=�5   	   ���#0864AC � 
ResizeMode$ NoResize=�5   	    � 	Resources 5       �� MergedDictionaries �� Source   ShadowBorderStyle �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect	   Color
   
BlurRadius   ShadowDepth   Opacity�� 5   
    5      �� 5      $ /Views/Designs/Style.xaml?�6%   %�   &	        �� 5      " ��6.   .�5      ��  �@J�Effect 5        5      $	 Black��6+   $
 10Q�69   $ 2Q�6I   $	 0.2Q�6Y   5      �� 5       �@��CornerRadius$ 10��61   5      5      .��5"      � $
��Left=�6   $
��1080��6&    � ColumnDefinitions 5#   
   �� 5$      $��3* �6   �� 5%      $��2* �6   .+�5)   
   �� ���#E6F0FA#��[  6&    �� Padding$ 30q�6Q   $��	10,0,10,0q�6^   $��Center=�6q   $	��586��6�   ._�5*      �� -   5         ��Name$ 	Positions��5*       �� View 5+      �� 5,      .P�5-      ��  �� Width$
 Auto��6)    �� DisplayMemberBinding 66   �� *Nombre+��5.      �� 5/   "   -   5        $��Puesto��5/   7   
 � Tag$ Nombre��6k   51      �� 52      $
 Auto��6)    66   �� *EstadoActivo+��53      �� 54   "   -   5        $��Activo��54   7   $ Activo��6k   56      58      5:      5;      �� 5>   
   $��1��6   $	��Top=�6%   $��
6,168,14,0q�6=   $	��383��6Q   .+�5?      �� -   5          ModernButtonStyle#��[ 5?      $��Agregar��6@   $��0,5,0,50q�6R   $��50��6d   $	��250��6p   6|   �� 5@      -   5        #��[ 5@      $��	Modificar��6@   $��0,5,0,50q�6T   $��50��6f   $	��250��6r   6~   �� 5A      -   5        #��[ 5A      $��Activar/Desactivar��6@   $��0,5,0,50q�6]   $��50��6o   $	��250��6{   6�   �� 5B      -   5        #��[ 5B      $��Volver��6@   $��10q�6Q   6]   5C      5D      5E      