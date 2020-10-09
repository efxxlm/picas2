import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { DialogCargarActaSuscritaConstComponent } from '../dialog-cargar-acta-suscrita-const/dialog-cargar-acta-suscrita-const.component';
export interface Contrato {
  idContrato: number;
  fechaAprobacionRequisitos: string;
  numeroContrato: string;
  estado: string;
  enviadoparaInterventor: boolean;
  actaSuscrita: boolean;
}



const ELEMENT_DATA: Contrato[] = [
  {
    idContrato: 1,
    fechaAprobacionRequisitos:"20/06/2020",
    numeroContrato:"C223456789",
    estado:"Sin validar",
    enviadoparaInterventor:null,
    actaSuscrita:null
  },
  {
    idContrato: 2,
    fechaAprobacionRequisitos:"21/06/2020",
    numeroContrato:"C223456790",
    estado:"Con observaciones",
    enviadoparaInterventor:null,
    actaSuscrita:null
  },
  {
    idContrato: 3,
    fechaAprobacionRequisitos:"22/06/2020",
    numeroContrato:"C223456791",
    estado:"Con observaciones",
    enviadoparaInterventor:true,
    actaSuscrita:null
  },
  {
    idContrato: 4,
    fechaAprobacionRequisitos:"23/06/2020",
    numeroContrato:"C223456791",
    estado:"Enviada al interventor",
    enviadoparaInterventor:null,
    actaSuscrita:null
  },
  {
    idContrato: 5,
    fechaAprobacionRequisitos:"24/06/2020",
    numeroContrato:"C223456792",
    estado:"Sin acta generada",
    enviadoparaInterventor:null,
    actaSuscrita:null
  },
  {
    idContrato: 6,
    fechaAprobacionRequisitos:"25/06/2020",
    numeroContrato:"C223456793",
    estado:"Con acta generada",
    enviadoparaInterventor:null,
    actaSuscrita:null
  },
  {
    idContrato: 7,
    fechaAprobacionRequisitos:"26/06/2020",
    numeroContrato:"C223456794",
    estado:"Con acta en proceso de firma",
    enviadoparaInterventor:null,
    actaSuscrita:null
  },
  {
    idContrato: 8,
    fechaAprobacionRequisitos:"27/06/2020",
    numeroContrato:"C223456795",
    estado:"Con acta suscrita y cargada",
    enviadoparaInterventor:null,
    actaSuscrita:true
  }
];
@Component({
  selector: 'app-tabla-contr-obra-fdos-constr',
  templateUrl: './tabla-contr-obra-fdos-constr.component.html',
  styleUrls: ['./tabla-contr-obra-fdos-constr.component.scss']
})
export class TablaContrObraFdosConstrComponent implements OnInit {
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
  validarActaParaInicio(id){
    localStorage.setItem("origin","obra");
    localStorage.setItem("editable","false");
    this.router.navigate(['/generarActaInicioConstruccion/validarActaDeInicio',id]);
  }
  verDetalleEditar(id){
    localStorage.setItem("origin","obra");
    localStorage.setItem("editable","true");
    this.router.navigate(['/generarActaInicioConstruccion/validarActaDeInicio',id]);
  }
  verDetalle(id){
    this.router.navigate(['/generarActaInicioConstruccion/verDetalleActaConstruccion',id]);
  }
  generarActaFDos(){
    this.router.navigate(['/generarActaInicioConstruccion/generarActa']);
  }
  cargarActaSuscrita(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '45%';
    const dialogRef = this.dialog.open(DialogCargarActaSuscritaConstComponent, dialogConfig);
  }
  descargarActaDesdeTabla(){
    alert("llama al servicio");
  }

}
