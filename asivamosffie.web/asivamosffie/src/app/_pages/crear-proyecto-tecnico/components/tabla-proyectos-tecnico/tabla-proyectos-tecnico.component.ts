import { Component, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { MatDialog } from '@angular/material/dialog';
import { DatePipe } from '@angular/common';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';


export interface RegistrosCargados {
  id: number;
  llaveMen: string;
  fecha: string,
  departamento: string,
  municipio: string,
  institucion: string,
  sede: string,
  estado: string,
  estadoj: string,
  estadop: string,
  gestion: any,
  proyectoDisponible: boolean
}

@Component({
  selector: 'app-tabla-proyectos-tecnico',
  templateUrl: './tabla-proyectos-tecnico.component.html',
  styleUrls: ['./tabla-proyectos-tecnico.component.scss']
})
export class TablaProyectosTecnicoComponent {
  //no se va a usar estado juridico
  //displayedColumns: string[] = ['fecha','departamento','municipio','institucion','sede','estado','estadoj','estadop','gestion'];

  displayedColumns: string[] =
    [
      'fecha',
      'llaveMen',
      'departamento',
      'municipio',
      'institucion',
      'sede',
      'estado',
      //'estadop',
      'gestion'
    ];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  dataSource = new MatTableDataSource();
  proyectoid: number;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(private projectService: ProjectService, public dialog: MatDialog,
    public datepipe: DatePipe, private router: Router,) {
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  openDialogSiNo(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this.projectService.deleteProjectById(this.proyectoid).subscribe(respuesta => {
          let proyecto = respuesta;
          if (respuesta) {
            this.inicializar();
            this.openDialog('', "<b>La informaci??n a sido eliminada correctamente.</b>");
          }
          else {
            this.openDialog('', "<b>El registro tiene informaci??n que depende de ??l no se puede eliminar.</b>");
          }
        },
          err => {
            let mensaje: string;
            console.log(err);
            if (err.message) {
              mensaje = err.message;
            }
            else if (err.error.message) {
              mensaje = err.error.message;
            }
            this.openDialog('Error', mensaje);
          },
          () => {
            // console.log('termin??');
          });
      }
    });
  }

  inicializar() {
    this.projectService.getListProjects().subscribe(respuesta => {
      respuesta.forEach(element => {
        element.fecha = element.fecha.split('T')[0].split('-').reverse().join('/');
      });
      let datos: RegistrosCargados[] = [];
      console.log(respuesta);
      respuesta.forEach(element => {
        datos.push({
          fecha: element.fecha,
          llaveMen: element.llaveMen,
          id: element.proyectoId,
          departamento: element.departamento,
          municipio: element.municipio,
          estado: element.estadoRegistro,
          estadoj: element.estadoJuridicoPredios,
          estadop: element.estadoProyecto,
          institucion: element.institucionEducativa,
          sede: element.sede,
          gestion:
          {
            id: element.proyectoId,
            estadoProyectoObra: element.estadoProyectoObra,
            estadoProyectoInterventoria: element.estadoProyectoInterventoria
          },
          proyectoDisponible: (element.codigoEstadoProyectoInterventoria === "9" 
                            || element.codigoEstadoProyectoInterventoria === "1"
                            || element.codigoEstadoProyectoInterventoria === "5"
                            ) 
                            && (element.codigoEstadoProyectoObra === "9" 
                             || element.codigoEstadoProyectoObra === "1"
                             || element.codigoEstadoProyectoObra === "5"
                             ) ? true : false
        });
      });

      this.dataSource = new MatTableDataSource<RegistrosCargados>(datos);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por p??gina';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
        if (length === 0 || pageSize === 0) {
          return '0 de ' + length;
        }
        length = Math.max(length, 0);
        const startIndex = page * pageSize;
        // If the start index exceeds the list length, do not try and fix the end index to the end.
        const endIndex = startIndex < length ?
          Math.min(startIndex + pageSize, length) :
          startIndex + pageSize;
        return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
      };
      this.paginator._intl.previousPageLabel = 'Anterior';
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        //console.log('termin??');
      });
  }
  ngOnInit(): void {
    this.inicializar();
  }

  ver(gestion: any, estado: boolean = false) {
    console.log(gestion);
    this.router.navigate(['/crearProyecto/crearProyecto', { id: gestion, estado: estado }]);
  }

  eliminar(gestion: any) {
    console.log(gestion);
    this.proyectoid = gestion;
    this.openDialogSiNo('', "<b>??Est?? seguro de eliminar este registro?</b>",);

  }

  enviar(gestion: any) {
    console.log(gestion);
  }
}
