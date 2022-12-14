import { GestionarActPreConstrFUnoService } from './../../../../core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DialogCargarActaSuscritaConstComponent } from '../dialog-cargar-acta-suscrita-const/dialog-cargar-acta-suscrita-const.component';
import { ActaInicioConstruccionService } from 'src/app/core/_services/actaInicioConstruccion/acta-inicio-construccion.service';
export interface Contrato {
  idContrato: number;
  fechaAprobacionRequisitos: string;
  numeroContrato: string;
  estado: string;
  enviadoparaInterventor: boolean;
  actaSuscrita: boolean; 
}
 
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
  TipoContratoInterventoria: string ="2";
  constructor(private router: Router, public dialog: MatDialog, private services: ActaInicioConstruccionService, private gestionarActaSvc: GestionarActPreConstrFUnoService) { }

  ngOnInit(): void {
 
    this.services.GetListGrillaActaInicio(8).subscribe((data:any) => {  
      for(let contratos of data){
       if(contratos.tipoContratoCodigo === this.TipoContratoInterventoria){
          this.dataTable.push(contratos);
         }
      }
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por p??gina';
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
    this.router.navigate(['/generarActaInicioConstruccion/generarActaFDos', id]);
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
  enviarActaParaFirma(id, numeroContrato) {
    //console.log(localStorage.getItem("origin"))
    //if (localStorage.getItem("origin") == "interventoria") {
      this.services.CambiarEstadoActa(id, "6", "usr2").subscribe(data => {
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/generarActaInicioConstruccion'])
        );
      });
      this.descargarActaDesdeTabla(id, numeroContrato);
    //}
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
      fecha1Titulo = 'Fecha de la firma del documento por parte del contratista de interventor??a';
      fecha2Titulo = 'Fecha de la firma del documento por parte del supervisor';
    }
    else{
      fecha1Titulo = 'Fecha de la firma del documento por parte del contratista de obra';
      fecha2Titulo = 'Fecha de la firma del documento por parte del contratista de interventor??a';
    }
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '1000px';
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
  descargarActaDesdeTabla(id, numContrato?) {
    this.gestionarActaSvc.GetActaByIdPerfil(id, 'True').subscribe(resp => {
      const documento = `${numContrato}.pdf`; // Valor de prueba
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
