import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { CargarActaSuscritaActaIniFIPreconstruccionComponent } from '../cargar-acta-suscrita-acta-ini-f-i-prc/cargar-acta-suscrita-acta-ini-f-i-prc.component';

export interface Contrato {
  fechaAprobacionRequisitos: string;
  numeroContrato:string;
  estado:string;
  enviadoparaInterventor:boolean;
  actaSuscrita:boolean;
}

const ELEMENT_DATA: Contrato[] = [
  {fechaAprobacionRequisitos:"20/06/2020",numeroContrato:"C223456789",estado:"Sin validar",enviadoparaInterventor:null,actaSuscrita:null},
  {fechaAprobacionRequisitos:"21/06/2020",numeroContrato:"C223456790",estado:"Con observaciones",enviadoparaInterventor:null,actaSuscrita:null},
  {fechaAprobacionRequisitos:"22/06/2020",numeroContrato:"C223456791",estado:"Con observaciones",enviadoparaInterventor:true,actaSuscrita:null},
  {fechaAprobacionRequisitos:"26/06/2020",numeroContrato:"C223456794",estado:"Con acta en proceso de firma",enviadoparaInterventor:null,actaSuscrita:null},
  {fechaAprobacionRequisitos:"27/06/2020",numeroContrato:"C223456795",estado:"Con acta suscrita y cargada",enviadoparaInterventor:null,actaSuscrita:true}
];
@Component({
  selector: 'app-tabla-actas-de-inicio-de-interventoria',
  templateUrl: './tabla-actas-de-inicio-de-interventoria.component.html',
  styleUrls: ['./tabla-actas-de-inicio-de-interventoria.component.scss']
})
export class TablaActasDeInicioDeInterventoriaComponent implements OnInit {

  displayedColumns: string[] = [ 'fechaAprobacionRequisitos', 'numeroContrato', 'estado', 'id'];
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
  validarActaParaInicio(){
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/validarActaDeInicio']);
  }
  verDetalleEditar(){
    localStorage.setItem("conObservaciones","true");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/verDetalleEditarActa']);
  }
  verDetalle(actaSuscrita){
    if(actaSuscrita == true){
      localStorage.setItem("actaSuscrita","true");
    }
    else{
      localStorage.setItem("actaSuscrita","false");
    }
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/verDetalleActa']);
  }
  generarActaFUno(){
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/generarActa']);
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
