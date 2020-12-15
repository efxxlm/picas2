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

  displayedColumns: string[] = ['fechaAprobacionRequisitos', 'numeroContratoObra', 'estadoActa', 'contratoId'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  dataTable:any = [];
  loadDataItems: Subscription;
  constructor(private router: Router, public dialog: MatDialog, private services: ActBeginService) { }

  ngOnInit(): void {
    /*
    this.loadDataItems = this.services.loadDataItems.subscribe((loadDataItems: any) => {
      if (loadDataItems != '') {
        this.dataTable = loadDataItems;
      }
    });
    */
    this.services.GetListGrillaActaInicio(8).subscribe((data:any) => {
      for(let contratos of data){
        if(contratos.tipoContrato == 'Interventoria'){
          this.dataTable.push(contratos);
        }
      }
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
    });
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  validarActaParaInicio(id) {
    localStorage.setItem("origin", "interventoria"); 
    localStorage.setItem("editable", "false");
    this.router.navigate(['/generarActaInicioConstruccion/generarActaFDos', id]);
  }
  verDetalleEditar(id) {
    localStorage.setItem("origin", "interventoria");
    localStorage.setItem("editable", "true");
    this.router.navigate(['/generarActaInicioConstruccion/validarActaDeInicio', id]);
  }
  verDetalle(id) {
    this.router.navigate(['/generarActaInicioConstruccion/verDetalleActaConstruccion', id]);
  }
  generarActaFDos(id) {
    this.router.navigate(['/generarActaInicioConstruccion/generarActaFDos', id]);
  }
  cambiarEstadoSupervisor(id) {
    if (localStorage.getItem("origin") == "interventoria") {
      this.services.CambiarEstadoActa(id, "3", "usr2").subscribe(data => {
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/generarActaInicioConstruccion'])
        );
      });
    }
  }
  cambiarEstadoInterventor(id, tieneObs) {
    if (localStorage.getItem("origin") == "interventoria") {
        this.services.CambiarEstadoActa(id, "5", "usr2").subscribe(data => {
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(
            () => this.router.navigate(['/generarActaInicioConstruccion'])
          );
        });
    }
  }
  enviarActaParaFirma(id) {
    if (localStorage.getItem("origin") == "interventoria") {
      this.services.CambiarEstadoActa(id, "6", "usr2").subscribe(data => {
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/generarActaInicioConstruccion'])
        );
      });
      this.descargarActaDesdeTabla(id);
    }
  }
  enviarInterventorBtn(id){
    if (localStorage.getItem("origin") == "interventoria") {
      this.services.CambiarEstadoActa(id, "4", "usr2").subscribe(data => {
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/generarActaInicioConstruccion'])
        );
      });
    }
  }
  cargarActaSuscrita(id,tipoContrato,numContrato) {
    let idRol = 8;
    let fecha1Titulo;
    let fecha2Titulo;
    if(tipoContrato=='Interventoria'){
      fecha1Titulo = 'Fecha de la firma del documento por parte del contratista de interventoría';
      fecha2Titulo = 'Fecha de la firma del documento por parte del supervisor';
    }
    else{
      fecha1Titulo = 'Fecha de la firma del documento por parte del contratista de obra';
      fecha2Titulo = 'Fecha de la firma del documento por parte del contratista de interventoría';
    }
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '865px';
    dialogConfig.data = {id:id, idRol:idRol, numContrato:numContrato, fecha1Titulo:fecha1Titulo, fecha2Titulo:fecha2Titulo};
    const dialogRef = this.dialog.open(DialogCargarActaSuscritaConstComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(value => {
      if (value == 'aceptado') {
        this.services.CambiarEstadoActa(id,"7","usr2").subscribe(data=>{
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(
            () => this.router.navigate(['/generarActaInicioConstruccion'])
          );
        });
      }
    });
  }
  descargarActaDesdeTabla(id) {
    this.services.GetPlantillaActaInicio(id).subscribe(resp => {
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
