   M S B A M L   `   `   `  ����  
ProdLogApp!    ProdLogApp.Views.AddProduct   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      -   5          � Title$  Agregar Producto��5      $	��400��5      $	��500��5       � WindowStartupLocation$ CenterScreen=�5	       � 
ResizeMode$ NoResize=�5
      ���#0864AC � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Views/Designs/Style.xaml?�6%   %    5      .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��1* �6   .+�5#   
   �� $��Categoría:��6   $��16(�6%   
�������$��0��5$      $
��Left=�6   $��
10,40,0,20q�68   �� 5'   
   -   5         ��Name$ SeleccionCategoria��5'      $��Seleccionada: Ninguna��5(      $��14(�60   
�������$��0��5)      $
��Left=�6   $��
97,42,0,37q�68   $	��353��6L   �� 5,   
   -   5           ModernButtonStyle#��[  5,      $��Seleccionar��6<   $��0��5-      $	��100��6   $��30��6*   $��Center=�66   $��0,93,0,2q�6S   5.      �� 51   
   $��Nombre del Producto:��6   $��16(�6/   
�������$��1��52      $
��Left=�6   $��138,7,0,219q�68   �� 55   
   -   5        	 ��Name$	 NombreProducto��55        ModernTextBoxStyle#��[ 56      $	��346��57      $��35��6   $��1��58      $
��Left=�6   $��48,35,0,197q�6:   �� 5;   
   
 �� Orientation$
 
Horizontal=�6   $��1��6.   $
��Left=�6;   $��91,121,0,89q�6V   .+�5<      �� -   5          ConfirmButtonStyle#��[ 5<      $��	Confirmar��6A   $	��120��5=      $��40��6!   $��10q�6-   69   �� 5>      -   5          CancelButtonStyle#��[ 5>      $��Cancelar��6@   $	��120��5?      $��40��6!   6-   5@      5A      5B      