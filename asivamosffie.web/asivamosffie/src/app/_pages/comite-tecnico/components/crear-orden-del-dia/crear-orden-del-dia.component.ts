import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { SolicitudesContractuales, ComiteTecnico, SesionComiteTema, EstadosComite, SesionComiteSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { forkJoin } from 'rxjs';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-crear-orden-del-dia',
  templateUrl: './crear-orden-del-dia.component.html',
  styleUrls: ['./crear-orden-del-dia.component.scss']
})

export class CrearOrdenDelDiaComponent implements OnInit {

  listaMiembros: Dominio[] = [];
  solicitudesContractuales: SolicitudesContractuales[] = [];
  fechaSesionString: string;
  fechaSesion: Date;
  idComite: number = 0;
  estadosComite = EstadosComite;
  objetoComiteTecnico: ComiteTecnico = {
    estadoComiteCodigo: this.estadosComite.sinConvocatoria
  };

  addressForm = this.fb.group({
    tema: this.fb.array([]),
  });



  responsablesArray = [
    { name: 'reponsable 1', value: '1' },
    { name: 'reponsable 2', value: '2' },
    { name: 'reponsable 3', value: '3' }
  ];

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private techicalCommitteeSessionService: TechnicalCommitteSessionService,
    private router: Router,
    private commonService: CommonService,

  ) {

  }


  ngOnInit(): void {
    this.activatedRoute.params.subscribe(a => {
      let fecha = Date.parse(a.fecha);
      this.fechaSesion = new Date(fecha);
      this.fechaSesionString = `${this.fechaSesion.getFullYear()}/${this.fechaSesion.getMonth() + 1}/${this.fechaSesion.getDate()}`

      this.idComite = a.id;

      if (this.idComite > 0) {
        this.editMode();
      } else {

        forkJoin([
          this.techicalCommitteeSessionService.getListSolicitudesContractuales(this.fechaSesionString),
          this.commonService.listaMiembrosComiteTecnico(),

        ]).subscribe(response => {

          this.solicitudesContractuales = response[0];

          this.solicitudesContractuales.forEach(sc => {
            sc.tipoSolicitudCodigo = sc.tipoSolicitudNumeroTabla

          })


          this.listaMiembros = response[1];

          setTimeout(() => {

            let btnTablaSolicitudes = document.getElementById('btnTablaSolicitudes');
            btnTablaSolicitudes.click();

          }, 1000);

        });
      }

    })
  }

  editMode() {

    forkJoin([
      //this.techicalCommitteeSessionService.getListSesionComiteTemaByIdSesion( this.idSesion ),
      this.techicalCommitteeSessionService.getComiteTecnicoByComiteTecnicoId(this.idComite),
      this.commonService.listaMiembrosComiteTecnico(),

    ]).subscribe(response => {

      this.objetoComiteTecnico = response[0];
      this.listaMiembros = response[1];
      console.log(response[0])
      this.solicitudesContractuales = response[0].sesionComiteSolicitudComiteTecnico;

      if (this.solicitudesContractuales) {
        this.solicitudesContractuales.forEach(sc => {
          sc.id = sc.solicitudId;
        })
      }

      setTimeout(() => {

        let btnTablaSolicitudes = document.getElementById('btnTablaSolicitudes');
        btnTablaSolicitudes.click();

      }, 1000);

      let temas = this.addressForm.get('tema') as FormArray;

      temas.clear();

      response[0].sesionComiteTema = response[0].sesionComiteTema.filter(t => t.esProposicionesVarios != true)

      response[0].sesionComiteTema.forEach(te => {
        let grupoTema = this.crearTema();
        let responsable = this.listaMiembros.find(m => m.codigo == te.responsableCodigo)

        grupoTema.get('tema').setValue(te.tema);
        grupoTema.get('responsable').setValue(responsable);
        grupoTema.get('tiempoIntervencion').setValue(te.tiempoIntervencion);
        grupoTema.get('url').setValue(te.rutaSoporte);
        grupoTema.get('sesionTemaId').setValue(te.sesionTemaId);


        temas.push(grupoTema);

      })

      console.log(response);
    })
  }

  eliminarTema(i) {
    let tema = this.addressForm.get('tema');
    this.openDialogSiNo('', '¿Está seguro de eliminar este registro?', i, tema);

  }

  deleteTema(i) {
    let grupo = this.addressForm.get('tema') as FormArray;
    let tema = grupo.controls[i];

    console.log(tema)

    this.techicalCommitteeSessionService.deleteSesionComiteTema(tema.get('sesionTemaId').value ? tema.get('sesionTemaId').value.sesionTemaId : 0)
      .subscribe(respuesta => {
        this.borrarArray(grupo, i)
        this.openDialog('', 'La información se ha eliminado correctamente.')
        this.ngOnInit();
      })

  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number, grupo: any) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result) {
        this.deleteTema(e)
      }
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  get tema() {
    return this.addressForm.get('tema') as FormArray;
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  agregaTema() {
    this.tema.push(this.crearTema());
  }

  getStyle() {
    if (this.estadosComite.sinConvocatoria == this.objetoComiteTecnico.estadoComiteCodigo)
      return 'auto'
    else
      return 'none'
  }

  crearTema() {
    return this.fb.group({
      sesionTemaId: [],
      tema: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      responsable: [null, Validators.required],
      tiempoIntervencion: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(3)])
      ],
      url: [null, [
        //Validators.required,
        //Validators.pattern('/^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/')
      ]],
    });
  }

  onSubmit() {

    console.log(this.addressForm);
    if (this.addressForm.invalid) {
      this.openDialog('Falta registrar información', '');

    } else {
      let comite: ComiteTecnico = {
        comiteTecnicoId: this.idComite,
        fechaOrdenDia: this.fechaSesion,
        sesionComiteTema: [],
        sesionComiteSolicitudComiteTecnico: [],

      }

      this.tema.controls.forEach(control => {
        let sesionComiteTema: SesionComiteTema = {
          tema: control.get('tema').value,
          responsableCodigo: control.get('responsable').value.codigo,
          tiempoIntervencion: control.get('tiempoIntervencion').value,
          rutaSoporte: control.get('url').value,
          sesionTemaId: control.get('sesionTemaId').value,
          comiteTecnicoId: this.idComite,

        }

        comite.sesionComiteTema.push(sesionComiteTema);
      })

      if (this.solicitudesContractuales) {
        this.solicitudesContractuales.forEach(sol => {
          let sesionComiteSolicitud: SesionComiteSolicitud = {
            comiteTecnicoId: this.idComite,
            solicitudId: sol.id,
            sesionComiteSolicitudId: sol.sesionComiteSolicitudId,
            tipoSolicitudCodigo: sol.tipoSolicitudCodigo,
          }

          comite.sesionComiteSolicitudComiteTecnico.push(sesionComiteSolicitud);
        })
      }

      console.log(comite)

      this.techicalCommitteeSessionService.createEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(comite).subscribe(respuesta => {
        this.openDialog('Sesion Comite', respuesta.message)
        if (respuesta.code == "200")
          this.router.navigate(['/comiteTecnico']);
      });
    }
  }
}
