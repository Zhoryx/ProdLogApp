   M S B A M L   `   `   `  ����  
ProdLogApp%    ProdLogApp.Views.UsuarioAgregar   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Aav2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6G     � Title$  Agregar Usuario��5   	   $	��400��5   	   $	��600��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5	   	   ���#0864ac � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Views/Designs/Style.xaml?�6%   %    5      .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5       $
��Auto �6    � ColumnDefinitions 5#   
   �� 5$      $
��Auto �6   �� 5%      $��* �6   .+�5+   
   �� $��Nombre��6   $��14(�6!   ���#FFFFFF$	��Top=�5,      $��4��6(   $��0��65   $��25,23,414,0q�5-      $��2��6%   �� 5.   
   -   5         ��Name$ NombreTextBox��5.         ModernTextBoxStyle#��[  6)   $	��300��5/      $��30��6   $��4��6*   $��1��67   $��130,23,130,205q�50      �� 53   
   $
��DNI:��6   $��14(�6   ���#FFFFFF$	��Top=�54      $��4��6(   $��0��65   $��26,82,414,0q�55      $��2��6%   �� 56   
   -   5        $ TextBox��56      #��[  6#   $	��300��57      $��30��6   $��4��6*   $��1��67   $��130,82,130,146q�58      �� 5:   
   $��Cuenta Gerencial?��6   $��14(�6,   ���#FFFFFF$	��Top=�5;      $��4��6%   $��0��62   $��25,143,414,0q�5<      $��2��6#   �� 5>   
   -   5        $ 
TextBoxDNI��5>      #��[  6&   $	��300��5?   	   $��30��6   $��4��6!   $��1��6.   $��130,82,130,146q�5@   	   �� 5B   
   -   5        	 ��Name$	 CheckBox��5B      $��Si��6%   $��280,149,232,91q�62   $��4��6J   $��1��6W   $��14(�6g   ���#FFFFFF6u   �� 5D   
   -   5        $ TextBoxPasswordGerente��5D      #��[  62   $	��300��5E   	   $��30��6   $��4��6!   $��1��6.   $��130,205,130,23q�5F   	   �� 5H   
   $��Contraseña��6   $��14(�6%   ���#FFFFFF$	��Top=�5I      $��4��6%   $��0��62   $��25,205,415,0q�5J      $��2��6#   �� 5M   
   
 �� Orientation$
 
Horizontal=�6   $��Center=�6.   $��4��5N      $��0,279,0,-127q�6   $��1��61   .+�5O      �� -   5          ConfirmButtonStyle#��[ 5O      $��	Confirmar��6A   $��10q�5P      6   �� 5Q      -   5          CancelButtonStyle#��[ 5Q      $��Cancelar��6@   $��10q�5R      6   5S      5U      5V      