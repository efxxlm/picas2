import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
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

  displayedColumns: string[] = [ 'fechaAprobacionRequisitos', 'numeroContratoObra', 'estadoActa', 'contratoId'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  dataTable: any[] = [];
  constructor(private router: Router, public dialog: MatDialog, private service: GestionarActPreConstrFUnoService) { }

  ngOnInit(): void {
    this.cargarTablaDeDatos();
  }
  cargarTablaDeDatos(){
    this.service.GetListGrillaActaInicio(8).subscribe((data:any)=>{
      for (let contrObras of data) {
        if (contrObras.tipoContratoNombre === 'Interventoria') {
          this.dataTable.push(contrObras);
        };
      };
      console.log(this.dataTable);
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
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/generarActa', id]);
  }
  verDetalleEditar(id) {
    localStorage.setItem("origin", "interventoria");
    localStorage.setItem("editable", "true");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/validarActaDeInicio', id]);
  }
  verDetalle(id) {
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/verDetalleActa', id]);
  }
  generarActaFDos(id) {
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/generarActaFDos', id]);
  }
  verDetalleActaCargada(id) {
    localStorage.setItem("actaSuscrita", "true");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/verDetalleActa', id]);
  }
  cambiarEstadoSupervisor(id) {
    if (localStorage.getItem("origin") == "interventoria") {
      this.service.CambiarEstadoActa(id, "3").subscribe(data => {
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
        );
      });
    }
  }
  cambiarEstadoInterventor(id, tieneObs) {
    if (localStorage.getItem("origin") == "interventoria") {
        this.service.CambiarEstadoActa(id, "5").subscribe(data => {
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(
            () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
          );
        });
    }
  }
  enviarActaParaFirma(id) {
    if (localStorage.getItem("origin") == "interventoria") {
      this.service.CambiarEstadoActa(id, "3").subscribe(data => {
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
        );
      });
      this.descargarActaDesdeTabla(id);
    }
  }
  enviarInterventorBtn(id){
    if (localStorage.getItem("origin") == "interventoria") {
      this.service.CambiarEstadoActa(id, "4").subscribe(data => {
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
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
    const dialogRef = this.dialog.open(CargarActaSuscritaActaIniFIPreconstruccionComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(value => {
      if (value == 'aceptado') {
        this.service.CambiarEstadoActa(id,"4").subscribe(data=>{
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(
            () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
          );
        });
      }
    });
  }
  descargarActaDesdeTabla(id) {
    this.service.GetActaByIdPerfil(8,id).subscribe(resp => {
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
