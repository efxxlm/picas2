import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { FuenteFinanciacion, Aportante, ProyectoAdministrativo, Listados, ProjectService, AportanteFuenteFinanciacion } from 'src/app/core/_services/project/project.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-formulario-proyectos',
  templateUrl: './formulario-proyectos.component.html',
  styleUrls: ['./formulario-proyectos.component.scss']
})
export class FormularioProyectosComponent implements OnInit {


  proyectoAdmin: ProyectoAdministrativo;
  listadoAportantes: Dominio[];
  listadoFuentes: Dominio[];

  addFont(index: number) {
    console.log("push");
    console.log(index);
    this.proyectoAdmin.proyectoAdministrativoAportante[index].aportanteFuenteFinanciacion.push({ valorFuente: null, fuenteRecursosCodigo: '',fuenteFinanciacionId:null,proyectoAdministrativoAportanteId:null });
  }

  deleteFont(key: AportanteFuenteFinanciacion, aportante: Aportante) {
    const index = this.proyectoAdmin.proyectoAdministrativoAportante.indexOf(aportante, 0);
    const index2 = this.proyectoAdmin.proyectoAdministrativoAportante[index].aportanteFuenteFinanciacion.indexOf(key, 0);
    if (index2 > -1) {
      this.proyectoAdmin.proyectoAdministrativoAportante[index].aportanteFuenteFinanciacion.splice(index2, 1);
    }
  }

  onchangeFont(i: number) {
    console.log(this.proyectoAdmin);
    console.log(i);
    this.projectServices.listaFuentes(this.proyectoAdmin.proyectoAdministrativoAportante[i].aportanteId).subscribe(respuesta => {
      this.listadoFuentes = respuesta;
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
        // console.log('terminó');
      });
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    console.log(id);
    this.projectServices.ListAdministrativeProject().subscribe(respuesta => {
      if(id!=null && id!="")
      {
        let proyectoadmin1=respuesta.filter(x=>x.proyectoAdminitracionId==id);
        
        this.proyectoAdmin = proyectoadmin1[0].proyecto;
        this.proyectoAdmin.identificador=proyectoadmin1[0].proyectoAdminitracionId;
        //this.proyectoAdmin.proyectoAdministrativoAportante=proyectoadmin1[0].proyecto.proyectoAdministrativoAportante;
        let i=0;
        proyectoadmin1[0].proyecto.proyectoAdministrativoAportante.forEach(element => {
          this.onchangeFont(i);
          i++
        });
        
        console.log(this.proyectoAdmin);
      }
      else{
        let idcontador = 0;
        idcontador = respuesta[0].proyectoAdminitracionId;
        this.proyectoAdmin = { identificador: (idcontador + 1).toString(), proyectoAdministrativoAportante: [{
          aportanteId: 0,
          proyectoAdminstrativoId: 0,
          
          aportanteFuenteFinanciacion: [{ valorFuente: null, fuenteRecursosCodigo: '',fuenteFinanciacionId:null,proyectoAdministrativoAportanteId:null,aportanteFuenteFinanciacionId:null }]
        }] };
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
        // console.log('terminó');
      });

    // this.listadoAportantes=[{id:"001",valor:"valor1"},{id:"002",valor:"valor2"}];
    this.commonServices.listaNombreTipoAportante().subscribe(respuesta => {

      this.listadoAportantes = respuesta.filter(x=>x.nombre=="FFIE");
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
        // console.log('terminó');
      });


    // this.listadoFuentes=[{id:"001",valor:"valor1"},{id:"002",valor:"valor2"}];

  }

  blockNumber(e: { keyCode: any; }) {
    const tecla = e.keyCode;
    if (tecla === 8 ) { return true; } // Tecla de retroceso (para poder borrar)
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
      aportanteId: 0,
      proyectoAdminstrativoId: 0,      
      aportanteFuenteFinanciacion: [{ valorFuente: 0, fuenteRecursosCodigo: '',fuenteFinanciacionId:null,proyectoAdministrativoAportanteId:null,aportanteFuenteFinanciacionId:null }]
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
              private router: Router) { }

  onSubmit() {
    this.projectServices.CreateOrUpdateAdministrativeProyect(this.proyectoAdmin).subscribe(respuesta => {
      this.openDialog('', respuesta.message,true);
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
        // console.log('terminó');
      });

  }

  openDialog(modalTitle: string, modalText: string,redirect?:boolean) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if(redirect)
    {
      dialogRef.afterClosed().subscribe(result => {
        if(result)
        {
          this.router.navigate(["/crearProyectoAdministrativo"], {});
        }
      });
    }
  }
}
