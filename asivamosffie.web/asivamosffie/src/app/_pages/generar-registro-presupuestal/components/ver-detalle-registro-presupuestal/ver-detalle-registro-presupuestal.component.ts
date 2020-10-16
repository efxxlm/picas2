import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

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
  selector: 'app-ver-detalle-registro-presupuestal',
  templateUrl: './ver-detalle-registro-presupuestal.component.html',
  styleUrls: ['./ver-detalle-registro-presupuestal.component.scss']
})
export class VerDetalleRegistroPresupuestalComponent implements OnInit {
  public numContrato = "A886675445";//valor quemado
  public fechaContrato = "20/06/2020";//valor quemado
  public solicitudContrato = "Modificación contractual";//valor quemado
  public estadoSolicitud = "Sin registro presupuestal";//valor quemado
  displayedColumns: string[] = ['componente', 'uso', 'valorUso', 'valorTotal'];
  dataSource = new MatTableDataSource();

  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(ELEMENT_DATA);
  }

}
