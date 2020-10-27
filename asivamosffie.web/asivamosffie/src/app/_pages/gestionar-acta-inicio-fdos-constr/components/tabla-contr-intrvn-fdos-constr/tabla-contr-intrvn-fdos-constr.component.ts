import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ActBeginService } from 'src/app/core/_services/actBegin/act-begin.service';
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
    fechaAprobacionRequisitos: "20/06/2020",
    numeroContrato: "C223456789",
    estado: "Sin validar",
    enviadoparaInterventor: null,
    actaSuscrita: null
  },
  {
    idContrato: 2,
    fechaAprobacionRequisitos: "21/06/2020", 
    numeroContrato: "C223456790", 
    estado: "Con observaciones", 
    enviadoparaInterventor: null, 
    actaSuscrita: null
  },
  { 
    idContrato: 3,
    fechaAprobacionRequisitos: "22/06/2020", 
    numeroContrato: "C223456791", 
    estado: "Con observaciones", 
    enviadoparaInterventor: true, 
    actaSuscrita: null 
  },
  { 
    idContrato: 4,
    fechaAprobacionRequisitos: "26/06/2020", 
    numeroContrato: "C223456794", 
    estado: "Con acta en proceso de firma", 
    enviadoparaInterventor: null, 
    actaSuscrita: null 
  },
  { 
    idContrato: 5,
    fechaAprobacionRequisitos: "27/06/2020", 
    numeroContrato: "C223456795", 
    estado: "Con acta suscrita y cargada", 
    enviadoparaInterventor: null, 
    actaSuscrita: true 
  }
];
@Component({
  selector: 'app-tabla-contr-intrvn-fdos-constr',
  templateUrl: './tabla-contr-intrvn-fdos-constr.component.html',
  styleUrls: ['./tabla-contr-intrvn-fdos-constr.component.scss']
})
export class TablaContrIntrvnFdosConstrComponent implements OnInit {

  displayedColumns: string[] = [ 'fechaAprobacionRequisitos', 'numeroContratoObra', 'estadoActa', 'contratoId'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  public dataTable;
  loadDataItems: Subscription;
  constructor(private router: Router, public dialog: MatDialog, private services: ActBeginService) { }

  ngOnInit(): void {
    this.loadDataItems = this.services.loadDataItems.subscribe((loadDataItems: any) => {
      if(loadDataItems!=''){
      this.dataTable=loadDataItems;
      }
    }); 
    this.services.GetListGrillaActaInicio().subscribe(data=>{
      this.dataTable = data;
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
      this.applyFilter("Interventoria");
    });
  }
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue;
  }
  validarActaParaInicio(id){
    localStorage.setItem("origin","interventoria");
    localStorage.setItem("editable","false");
    this.router.navigate(['/generarActaInicioConstruccion/generarActaFDos',id]);
  }
  verDetalleEditar(id){
    localStorage.setItem("origin","interventoria");
    localStorage.setItem("editable","true");
    this.router.navigate(['/generarActaInicioConstruccion/generarActaFDos',id]);
  }
  verDetalle(id){
    this.router.navigate(['/generarActaInicioConstruccion/verDetalleActaConstruccion',id]);
  }
  generarActaFDos(id) {
    this.router.navigate(['/generarActaInicioConstruccion/generarActaFDos',id]);
  }
  enviarActaParaFirma(id){
    this.services.CambiarEstadoActa(id,"4","usr2").subscribe(data=>{
      this.ngOnInit();
    });
  }
  cargarActaSuscrita(id) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '45%';
    dialogConfig.data = {id:id};
    const dialogRef = this.dialog.open(DialogCargarActaSuscritaConstComponent, dialogConfig);
  }
  descargarActaDesdeTabla(id){
    this.services.GetPlantillaActaInicio(id).subscribe(resp=>{
      const documento = `Prueba.pdf`; // Valor de prueba
      const text = documento,
      blob = new Blob([resp], { type: 'application/pdf' }),
      anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    });
  }
}
