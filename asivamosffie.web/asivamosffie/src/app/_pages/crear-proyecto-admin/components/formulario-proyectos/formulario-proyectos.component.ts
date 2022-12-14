import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { FuenteFinanciacion, Aportante, ProyectoAdministrativo, Listados, ProjectService, AportanteFuenteFinanciacion } from 'src/app/core/_services/project/project.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-formulario-proyectos',
  templateUrl: './formulario-proyectos.component.html',
  styleUrls: ['./formulario-proyectos.component.scss']
})
export class FormularioProyectosComponent implements OnInit {

  /*con este bit controlo los botones, esto lo hago ya sea por el estado del proyecto o en un futuro por el
    permiso que tenga el usuario
    */
  bitPuedoEditar = true;
  proyectoAdmin: ProyectoAdministrativo;
  listadoAportantes: Dominio[];
  listadoFuentes: any[];
  listadoFuentesArr: any[];
  estaEditando = false;
  lockFormFields: boolean = true;
  esVerDetalle: boolean;

  addFont(index: number) {
    // console.log("push");
    // console.log(index);
    this.proyectoAdmin.proyectoAdministrativoAportante[index].aportanteFuenteFinanciacion.push({ valorFuente: null, fuenteRecursosCodigo: null, fuenteFinanciacionId: null, proyectoAdministrativoAportanteId: null });
  }

  openDialogSiNo(modalTitle: string, modalText: string, key: AportanteFuenteFinanciacion, aportante: Aportante, fuenteFinanciacionId) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);

      if (result === true) {
        const index = this.proyectoAdmin.proyectoAdministrativoAportante.indexOf(aportante, 0);
        const index2 = this.proyectoAdmin.proyectoAdministrativoAportante[index].aportanteFuenteFinanciacion.indexOf(key, 0);

        if (fuenteFinanciacionId) {
          const fuenteRecursosString = this.listadoFuentesArr.find(e => e.fuenteFinanciacionId == fuenteFinanciacionId);
          this.listadoFuentes.push(fuenteRecursosString);
        }

        if (index2 > -1)
        {
          if (this.proyectoAdmin.proyectoAdministrativoAportante[index].aportanteFuenteFinanciacion[index2].aportanteFuenteFinanciacionId > 0) {
            this.projectServices.deleteProyectoFont(this.proyectoAdmin.proyectoAdministrativoAportante[index].aportanteFuenteFinanciacion[index2].aportanteFuenteFinanciacionId).subscribe();
          }
          this.proyectoAdmin.proyectoAdministrativoAportante[index].aportanteFuenteFinanciacion.splice(index2, 1);
        }
      }
    });
  }
  deleteFont(key: AportanteFuenteFinanciacion, aportante: Aportante, fuenteFinanciacionId) {

    this.openDialogSiNo("", "??Est?? seguro de eliminar este  registro?", key, aportante, fuenteFinanciacionId);
  }

  onchangeFont(i: number) {
    // console.log(this.proyectoAdmin);
    // console.log(i);
    this.projectServices.listaFuentes(this.proyectoAdmin.proyectoAdministrativoAportante[i].aportanteId).subscribe(respuesta => {
      this.listadoFuentesArr = respuesta;
      this.listadoFuentes = [...this.listadoFuentesArr];
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

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.projectServices.ListAdministrativeProject().subscribe(respuesta => {
      if (id != null && id != "") {
        let proyectoadmin1 = respuesta.filter(x => x.proyectoAdminitracionId == id);

        this.proyectoAdmin = proyectoadmin1[0].proyecto;
        this.proyectoAdmin.identificador = proyectoadmin1[0].proyectoAdminitracionId;
        //this.proyectoAdmin.proyectoAdministrativoAportante=proyectoadmin1[0].proyecto.proyectoAdministrativoAportante;
        let i = 0;
        proyectoadmin1[0].proyecto.proyectoAdministrativoAportante.forEach(element => {
          this.onchangeFont(i);
          i++
        });
        if (this.proyectoAdmin.enviado) {
          this.bitPuedoEditar = false;
        }
      }
      else {
        let idcontador = 0;
        idcontador = respuesta[0] ? respuesta[0].proyectoAdminitracionId : 0;
        this.proyectoAdmin = {
          identificador: (idcontador + 1).toString(), proyectoAdministrativoAportante: [{
            aportanteId: null,
            proyectoAdminstrativoId: null,

            aportanteFuenteFinanciacion: [{ valorFuente: null, fuenteRecursosCodigo: null, fuenteFinanciacionId: null, proyectoAdministrativoAportanteId: null, aportanteFuenteFinanciacionId: null }]
          }]
        };
      }
      if(!this.esVerDetalle){
        this.estaEditando = true;
        this.lockFormFields = false;
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

    // this.listadoAportantes=[{id:"001",valor:"valor1"},{id:"002",valor:"valor2"}];
    this.commonServices.listaNombreTipoAportante().subscribe(respuesta => {

      this.listadoAportantes = respuesta.filter(x => x.nombre == "FFIE");
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


    // this.listadoFuentes=[{id:"001",valor:"valor1"},{id:"002",valor:"valor2"}];

  }

  blockNumber(e: { keyCode: any; }) {
    const tecla = e.keyCode;
    if (tecla === 8) { return true; } // Tecla de retroceso (para poder borrar)
    if (tecla === 48) { return true; } // 0
    if (tecla === 49) { return true; } // 1
    if (tecla === 50) { return true; } // 2
    if (tecla === 51) { return true; } // 3
    if (tecla === 52) { return true; } // 4
    if (tecla === 53) { return true; } // 5
    if (tecla === 54) { return true; } // 6
    if (tecla === 55) { return true; } // 7
    if (tecla === 56) { return true; } // 8
    if (tecla === 57) { return true; } // 9
    const patron = /1/; // ver nota
    const te = String.fromCharCode(tecla);
    return patron.test(te);
  }

  addAportant() {
    this.proyectoAdmin.proyectoAdministrativoAportante.push({
      aportanteId: null,
      proyectoAdminstrativoId: null,
      aportanteFuenteFinanciacion: [{ valorFuente: null, fuenteRecursosCodigo: null, fuenteFinanciacionId: null, proyectoAdministrativoAportanteId: null, aportanteFuenteFinanciacionId: null }]
    });
  }
  deleteAportant(key: Aportante) {
    const index = this.proyectoAdmin.proyectoAdministrativoAportante.indexOf(key, 0);
    if (index > -1) {
      this.proyectoAdmin.proyectoAdministrativoAportante.splice(index, 1);
    }
  }


  constructor(private fb: FormBuilder,
    public commonServices: CommonService,
    public dialog: MatDialog,
    public projectServices: ProjectService,
    private route: ActivatedRoute,
    private router: Router) {
      this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
        if ( urlSegment.path === 'crearProyecto' ) {
            this.esVerDetalle = false;
            return;
        }
        if ( urlSegment.path === 'verDetalleProyecto' ) {
            this.esVerDetalle = true;
            return;
        }
      });
    }

  onSubmit() {
    this.estaEditando = true;
    this.projectServices.CreateOrUpdateAdministrativeProyect(this.proyectoAdmin).subscribe(respuesta => {
      this.openDialog('', `<b>${respuesta.message}</b>`, true);
    },
      err => {
        let mensaje: string;
        console.log(err);
        let msn = '';
        if (err.error.code === '501') {
          err.error.data.forEach(element => {
            msn += 'El campo ' + element.errors.key;
            element.errors.forEach(element => {
              msn += element.errorMessage + ' ';
            });
          });
        }
        if (err.error.message) {
          mensaje = err.error.message;
        }
        else if (err.message) {
          mensaje = err.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('termin??');
      });

  }

  openDialog(modalTitle: string, modalText: string, redirect?: boolean) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if (redirect) {
      dialogRef.afterClosed().subscribe(result => {

        this.router.navigate(["/crearProyectoAdministrativo"], {});

      });
    }
  }

  onchangeFuentes(event, index) {
    const fuenteFinanciacionId = event.value;

    for (let i = 0; i < this.listadoFuentes.length; i++) {
      const element = this.listadoFuentes[i];
      if (element.fuenteFinanciacionId === fuenteFinanciacionId) this.listadoFuentes.splice(i, 1);
    }
  }

  nombreFuente(fuenteFinanciacionId) {
    if(this.listadoFuentesArr != undefined && this.listadoFuentesArr != null){
      const fuenteRecursosString = this.listadoFuentesArr.find(e => e.fuenteFinanciacionId == fuenteFinanciacionId);
      return fuenteRecursosString.fuenteRecursosString;
    }else{
      return '';
    }
  }

  cofinanciacionIdFuente(fuenteFinanciacionId) {
    if(this.listadoFuentesArr != undefined && this.listadoFuentesArr != null){
      const cofinanciacionId = this.listadoFuentesArr.find(e => e.fuenteFinanciacionId == fuenteFinanciacionId);
      return cofinanciacionId.aportante ? cofinanciacionId.aportante?.cofinanciacionId : '';
    }else{
      return '';
    }
  }
}
