import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { CargarActaSuscritaActaIniFIPreconstruccionComponent } from '../cargar-acta-suscrita-acta-ini-f-i-prc/cargar-acta-suscrita-acta-ini-f-i-prc.component';
@Component({
  selector: 'app-tabla-generar-f-i-prc',
  templateUrl: './tabla-generar-f-i-prc.component.html',
  styleUrls: ['./tabla-generar-f-i-prc.component.scss']
})
export class TablaGenerarFIPreconstruccionComponent implements OnInit {
  displayedColumns: string[] = ['fechaAprobacionRequisitos', 'numeroContratoObra', 'estadoActa', 'contratoId'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  dataTable: any[] = [];

  public showSendForRevisionBtn: boolean = true;

  constructor(private router: Router, public dialog: MatDialog, private service: GestionarActPreConstrFUnoService) { }

  ngOnInit(): void {
    this.cargarTablaDeDatos();
  }
  cargarTablaDeDatos() {
    this.service.GetListGrillaActaInicio(2).subscribe((data:any) => {
      for (let actas of data){
        if (actas.tipoContratoNombre === 'Obra'){
          this.dataTable.push(actas);
        }
      }
      console.log(this.dataTable);
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
    })

  }
  generarActaFDos(id, tipoContrato) {
    if (tipoContrato == 'Interventoria') {
      localStorage.setItem("origin", "interventoria");
    }
    else {
      localStorage.setItem("origin", "obra");
    }
    localStorage.setItem("editable", "false");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/generarActa', id]);
  }
  verDetalleEditarActaFDos(id, tipoContrato) {
    if (tipoContrato == 'Interventoria') {
      localStorage.setItem("origin", "interventoria");
    }
    else {
      localStorage.setItem("origin", "obra");
    }
    localStorage.setItem("editable", "true");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/verDetalleEditarActa', id]);
  }
  enviarParaRevision(idContrato) {
    this.service.CambiarEstadoActa(idContrato, "14").subscribe(data => {
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(
        () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
      );
    });
  }
  verDetalleActaFDos(id) {
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/verDetalleActa', id]);
  }
  enviarActaParaFirma(id) {
    this.descargarActaDesdeTabla(id);
    this.service.CambiarEstadoActa(id, "19").subscribe(data => {
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(
        () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
      );
    });
  }
  enviarRevisionAprobacionInt(id) {
    this.service.CambiarEstadoActa(id, "3").subscribe(data => {
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(
        () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
      );
    });
  }
  enviarRevisionAprobacionTecEst2(id) {
    this.service.CambiarEstadoActa(id, "2").subscribe(data => {
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(
        () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
      );
    });
  }
  verDetalleActaCargada(id) {
    localStorage.setItem("actaSuscrita", "true");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/verDetalleActa', id]);
  }
  cargarActaSuscrita(id, tipoContrato, numContrato) {
    let idRol = 2;
    let fecha1Titulo;
    let fecha2Titulo;
    if (tipoContrato == 'Interventoria') {
      fecha1Titulo = 'Fecha de la firma del documento por parte del contratista de interventoría';
      fecha2Titulo = 'Fecha de la firma del documento por parte del supervisor';
    }
    else {
      fecha1Titulo = 'Fecha de la firma del documento por parte del contratista de obra';
      fecha2Titulo = 'Fecha de la firma del documento por parte del contratista de interventoría';
    }
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '45%';
    dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
    const dialogRef = this.dialog.open(CargarActaSuscritaActaIniFIPreconstruccionComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(value => {
      if (value == 'aceptado') {
        if (tipoContrato == 'Obra') {
          this.service.CambiarEstadoActa(id, "20").subscribe(data => {
            this.router.navigateByUrl('/', { skipLocationChange: true }).then(
              () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
            );
          });
        }
        else {
          this.service.CambiarEstadoActa(id, "7").subscribe(data0 => {
            this.router.navigateByUrl('/', { skipLocationChange: true }).then(
              () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
            );
          });
        }
      }
    });
  }
  descargarActaDesdeTabla(id) {
    this.service.GetActaByIdPerfil(2, id).subscribe(resp => {
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
