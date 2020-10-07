import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { CargarActaSuscritaActaIniFIPreconstruccionComponent } from '../cargar-acta-suscrita-acta-ini-f-i-prc/cargar-acta-suscrita-acta-ini-f-i-prc.component';

export interface Contrato {
  fechaAprobacionSupervisor: string;
  numeroContrato: string;
  estado: string;
  actaGenerada: boolean;
  revisionOAprobacion:boolean;
  observaciones:boolean;
  actaSuscrita: boolean;
}

const ELEMENT_DATA: Contrato[] = [
  {fechaAprobacionSupervisor: "20/06/2020", numeroContrato: 'A886675445',estado:'Sin acta generada',actaGenerada:false,revisionOAprobacion:false,observaciones:false,actaSuscrita:false},
  {fechaAprobacionSupervisor: "21/06/2020", numeroContrato: 'C223456789',estado:'Sin acta generada',actaGenerada:true,revisionOAprobacion:false,observaciones:false,actaSuscrita:false},
  {fechaAprobacionSupervisor: "10/06/2020", numeroContrato: 'C848784551',estado:'Sin acta generada',actaGenerada:false,revisionOAprobacion:false,observaciones:false,actaSuscrita:false},
  {fechaAprobacionSupervisor: "18/06/2020", numeroContrato: 'C848784552',estado:'Con acta preliminar generada',actaGenerada:null,revisionOAprobacion:true,observaciones:false,actaSuscrita:false},
  {fechaAprobacionSupervisor: "21/06/2020", numeroContrato: 'C848784553',estado:'Con validación del supervisor',actaGenerada:null,revisionOAprobacion:true,observaciones:false,actaSuscrita:false},
  {fechaAprobacionSupervisor: "24/06/2020", numeroContrato: 'C848784554',estado:'Con acta en proceso de firma',actaGenerada:null,revisionOAprobacion:null,observaciones:false,actaSuscrita:false},
  {fechaAprobacionSupervisor: "26/06/2020", numeroContrato: 'C848784555',estado:'Enviada por el supervisor',actaGenerada:null,revisionOAprobacion:null,observaciones:true,actaSuscrita:false},
  {fechaAprobacionSupervisor: "26/06/2020", numeroContrato: 'C848784555',estado:'Con acta suscrita y cargada',actaGenerada:null,revisionOAprobacion:null,observaciones:true,actaSuscrita:true},
];
@Component({
  selector: 'app-tabla-generar-f-i-prc',
  templateUrl: './tabla-generar-f-i-prc.component.html',
  styleUrls: ['./tabla-generar-f-i-prc.component.scss']
})
export class TablaGenerarFIPreconstruccionComponent implements OnInit {
  displayedColumns: string[] = [ 'fechaAprobacionSupervisor', 'numeroContrato', 'estado', 'id'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  constructor(private router: Router, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(ELEMENT_DATA);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }
  generarActaFUno(){
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/generarActa']);
  }
  verDetalleEditarActaFUno(observaciones){
    if(observaciones == true){
      localStorage.setItem("conObservaciones","true");
    }
    else{
      localStorage.setItem("conObservaciones","false");
    }
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/verDetalleEditarActa']);
  }
  enviarParaRevision(){
    alert("llama al servicio");
  }
  verDetalleActaFUno(observaciones,actaSuscrita){
    if(observaciones == true){
      localStorage.setItem("conObservaciones","true");
    }
    else{
      localStorage.setItem("conObservaciones","false");
    }
    if(actaSuscrita == true){
      localStorage.setItem("actaSuscrita","true");
    }
    else{
      localStorage.setItem("actaSuscrita","false");
    }
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/verDetalleActa']);
  }
  enviarActaParaFirma(){
    alert("llama al servicio donde cambia estado a true");
  }
  cargarActaSuscrita(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '45%';
    const dialogRef = this.dialog.open(CargarActaSuscritaActaIniFIPreconstruccionComponent, dialogConfig);
  }
  descargarActaDesdeTabla(){
    alert("llama al servicio");
  }
}
