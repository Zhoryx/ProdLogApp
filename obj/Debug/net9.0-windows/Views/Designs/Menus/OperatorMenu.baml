   M S B A M L   `   `   `  ����E  AProdLogApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ProdLogApp.ViewsProdLogApp.Views  5   	   #    ProdLogApp.Views.OperatorMenu   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:ProdLogApp.Views     5   	     � Title$  Menu - Producciones��5   	   $	��746��5	   	   $
��1080��5
   	    � WindowStartupLocation$ CenterScreen=�5   	   ���#0864AC � 
ResizeMode$ NoResize=�5   	    � 	Resources 5       �� MergedDictionaries �� Source   ShadowBorderStyle �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect	   Color
   
BlurRadius   ShadowDepth   Opacity�� 5   
    5      �� 5      $ /Views/Designs/Style.xaml?�6%   %�   &	        �� 5      " ��6.   .�5      ��  �@J�Effect 5        5      $	 Black��6+   $
 10Q�69   $ 2Q�6I   $	 0.2Q�6Y   5      �� 5       �@��CornerRadius$ 10��61   5      5      .��5"      � $��Center=�6   $
��1080��6(   $	��712��65   $	��Top=�6B    � ColumnDefinitions 5#   
   �� 5$      $��3* �6   �� 5%      $��2* �6   .+�5)   
   �� ���#E6F0FA#��[  6&    �� Padding$ 30q�6Q   $��	16,10,4,0q�6^   $	��Top=�6q   $	��625��6�   ._�5*      �� -   5         ��Name$ Productions��5*      $	��526��6,   $	��Top=�69    �� View 5+      �� 5,      .P�5-      ��  �� Width$
 Auto��6)   $��Producto��66    �� DisplayMemberBinding 6H   �� *
Producto+�� 5.      $
 Auto��6)   $��Cantidad��66    6H   �� *
Cantidad+�� 5/      $
 Auto��6)   $��⏰ Hora Inicio��66    6M   �� *	HInicio+ �� StringFormat$ HH:mm���� 50      $
 Auto��6)   $��⏳ Hora Fin��66    6J   �� *HFin+$ HH:mm��51      53      54      �� 57   
   $��1��6   $	��Top=�6%   $��
10,81,10,0q�6=   $	��286��6Q   .+�58      �� -   5          ModernButtonStyle#��[ 58      $��Agregar��6@   $��0,5,0,50q�6R   $��50��6d   $	��250��6p   6|   �� 59      #��[ 6   $��Eliminar��6@   $��0,5,0,50q�6S   $��50��6e   $	��250��6q   �� 5:      -   5        #��[ 5:      $��	Modificar��6@   $��0,5,0,50q�6T   $��50��6f   $	��250��6r   6~   5;      �� 5>   
   $��1��6   $	��Top=�6%    �� Orientation$ 
Horizontal=�6=   $��Center=�6V   $��	0,396,0,0q�6s    J� RenderTransformOrigin$ 0.48,-2.151&�6�   .+�5?      �� -   5          ConfirmButtonStyle#��[ 5?      $��	Confirmar��6A   6U   �� 5@        CancelButtonStyle#��[ 6   $��Cancelar��6@   $��10q�6S   5A      �� 5D   
   -   5        #��[ 5D      $��Desconectar��6<   $��50��6R   $	��250��6^   $
��Left=�6�   $��16,652,0,10q�6�   5E      5F      