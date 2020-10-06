import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogRef, MatDialogConfig, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CancelarDrpComponent } from '../cancelar-drp/cancelar-drp.component';

export interface tablaEjemplo {
  componente: string;
  uso: any[];
  valorUso: any[];
  valorTotal: string;
}

const ELEMENT_DATA: tablaEjemplo[] = [
  {
    componente: "Obra", uso: [
      { nombre: "Diseño" }, { nombre: "Diagnostico" }, { nombre: "Obra Principal" }
    ], valorUso: [
      { valor: "$ 8.000.000" }, { valor: "$ 12.000.000" }, { valor: "$ 60.000.000" }
    ], valorTotal: '$80.000.000'
  },
];
@Component({
  selector: 'app-gestionar-drp',
  templateUrl: './gestionar-drp.component.html',
  styleUrls: ['./gestionar-drp.component.scss']
})
export class GestionarDrpComponent implements OnInit {
  public numContrato = "A886675445";//valor quemado
  public fechaContrato = "20/06/2020";//valor quemado
  public solicitudContrato = "Modificación contractual";//valor quemado
  public estadoSolicitud = "Sin registro presupuestal";//valor quemado
  displayedColumns: string[] = ['componente', 'uso', 'valorUso', 'valorTotal'];
  dataSource = new MatTableDataSource();
  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(ELEMENT_DATA);
  }
  cancelarDRPBoton(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '50%';
    const dialogRef = this.dialog.open(CancelarDrpComponent, dialogConfig);
  }
  descargarDDPBoton(){
    console.log("llama al servicio");
  }
  generarDRPBoton(){

  }
}
