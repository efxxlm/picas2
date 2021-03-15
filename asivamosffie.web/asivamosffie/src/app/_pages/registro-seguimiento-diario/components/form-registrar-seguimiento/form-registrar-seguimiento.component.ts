import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { SeguimientoDiario } from 'src/app/_interfaces/DailyFollowUp';
import { FollowUpDailyService } from 'src/app/core/_services/dailyFollowUp/daily-follow-up.service';
import { forkJoin } from 'rxjs';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-form-registrar-seguimiento',
  templateUrl: './form-registrar-seguimiento.component.html',
  styleUrls: ['./form-registrar-seguimiento.component.scss']
})
export class FormRegistrarSeguimientoComponent implements OnInit {

  seguimientoId?: number;
  seguimiento: SeguimientoDiario;

  addressForm = this.fb.group({
    fechaSeguimiento: [null, Validators.required],
    disponibilidadPersonal: [null, Validators.required],
    cantidadPersonalOperativoProgramado: [null, Validators.compose([
      Validators.required, Validators.maxLength(3), Validators.max(999)])
    ],
    cantidadPersonalOperativoTrabajando: [null, Validators.compose([
      Validators.required, Validators.maxLength(3), Validators.max(999)])
    ],
    retrasoPersonal: [null, Validators.required],
    horasRetrasoPersonal:  [null, Validators.compose([
      Validators.required, Validators.maxLength(1), Validators.min(1), Validators.max(9)])
    ],
    disponibilidadPersonalObservaciones: [null, Validators.required],
    disponibilidadMaterial: [null, Validators.required],
    causaMaterial: [null, Validators.required],
    retrasoMaterial: [null, Validators.required],
    horasRetrasoMaterial:  [null, Validators.compose([
      Validators.required, Validators.maxLength(1), Validators.min(1), Validators.max(9)])
    ],
    disponibilidadMaterialObservaciones: [null, Validators.required],
    disponibilidadEquipo: [null, Validators.required],
    causaEquipo: [null, Validators.required],
    retrasoEquipo: [null, Validators.required],
    horasRetrasoEquipo:  [null, Validators.compose([
      Validators.required, Validators.maxLength(1), Validators.min(1), Validators.max(9)])
    ],
    disponibilidadEquipoObservaciones: [null, Validators.required],
    Productividad: [null, Validators.required],
    causaProductividad: [null, Validators.required],
    retrasoProductividad: [null, Validators.required],
    horasRetrasoProductividad:  [null, Validators.compose([
      Validators.required, Validators.maxLength(1), Validators.min(1), Validators.max(9)])
    ],
    ProductividadObservaciones: [null, Validators.required]
  });

  minDate: Date;
  editorStyle = {
    height: '45px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  personalArray = [
     { name: 'Suficiente', value: true },
     { name: 'Insuficiente', value: false }
  ];
  materialArray = [];
  equipolArray = [];
  productividadArray = [];
  causaBajaDisponibilidadMaterial = [];
  causaBajaDisponibilidadEquipo = [];
  causaBajaDisponibilidadProductividad = [];


  maxLength(e: any, n: number) {
    console.log(e.editor.getLength()+" "+n);
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n-1, e.editor.getLength());
    }
  }
  textoLimpio(texto,n) {
    if (texto!=undefined) {
      return texto.getLength() > n ? n : texto.getLength();
    }
  }

  proyecto: any;
  estaEditando = false;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private dailyFollowUpService: FollowUpDailyService,
    private commonServcie: CommonService,


  ) {

    if (this.router.getCurrentNavigation().extras.replaceUrl) {
      this.router.navigateByUrl('/registroSeguimientoDiario');
      return;
    };

    this.minDate = new Date();
    if (this.router.getCurrentNavigation().extras.state)
      this.proyecto = this.router.getCurrentNavigation().extras.state.proyecto;

    console.log( this.proyecto )
  }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.seguimientoId = params.id;
      console.log(this.seguimientoId, this.router.getCurrentNavigation());

      forkJoin(
        this.commonServcie.listaDisponibilidadMaterial(),
        this.commonServcie.listaDisponibilidadEquipo(),
        this.commonServcie.listaProductividad(),
        this.commonServcie.listaCausaBajaDisponibilidadMaterial(),
        this.commonServcie.listaCausaBajaDisponibilidadEquipo(),
        this.commonServcie.listaCausaBajaDisponibilidadProductividad(), 
        this.dailyFollowUpService.getDatesAvailableByContratacioProyectoId( this.proyecto.contratacionProyectoId )

      ).subscribe( respuesta => {
        this.materialArray = respuesta[0];
        this.equipolArray = respuesta[1];
        this.productividadArray = respuesta[2];
        this.causaBajaDisponibilidadMaterial = respuesta[3];
        this.causaBajaDisponibilidadEquipo = respuesta[4];
        this.causaBajaDisponibilidadProductividad = respuesta[5];
        this.diasPermitidos = respuesta[6];

        if (this.seguimientoId > 0)
        this.editMode()

      });



      
    });
  }

  editMode(){
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    this.dailyFollowUpService.getDailyFollowUpById( this.seguimientoId )
      .subscribe( seguimiento => {

        this.seguimiento = seguimiento;
        
        this.addressForm.setValue(
          {
            fechaSeguimiento:  seguimiento.fechaSeguimiento,    
            
            disponibilidadPersonal:               seguimiento.disponibilidadPersonal !== undefined ? seguimiento.disponibilidadPersonal : null,
            cantidadPersonalOperativoProgramado:  seguimiento.cantidadPersonalProgramado !== undefined ? seguimiento.cantidadPersonalProgramado : null,
            cantidadPersonalOperativoTrabajando:  seguimiento.cantidadPersonalTrabajando !== undefined ? seguimiento.cantidadPersonalTrabajando : null,
            retrasoPersonal:                      seguimiento.seGeneroRetrasoPersonal !== undefined ? seguimiento.seGeneroRetrasoPersonal : null,
            horasRetrasoPersonal:                 seguimiento.numeroHorasRetrasoPersonal !== undefined ? seguimiento.numeroHorasRetrasoPersonal : null,
            disponibilidadPersonalObservaciones:  seguimiento.disponibilidadPersonalObservaciones !== undefined ? seguimiento.disponibilidadPersonalObservaciones : null,

            disponibilidadMaterial:               seguimiento.disponibilidadMaterialCodigo !== undefined ? seguimiento.disponibilidadMaterialCodigo : null,
            causaMaterial:                        seguimiento.causaIndisponibilidadMaterialCodigo !== undefined ? seguimiento.causaIndisponibilidadMaterialCodigo : null,
            retrasoMaterial:                      seguimiento.seGeneroRetrasoMaterial !== undefined ? seguimiento.seGeneroRetrasoMaterial : null,
            horasRetrasoMaterial:                 seguimiento.numeroHorasRetrasoMaterial !== undefined ? seguimiento.numeroHorasRetrasoMaterial : null,
            disponibilidadMaterialObservaciones:  seguimiento.disponibilidadMaterialObservaciones !== undefined ? seguimiento.disponibilidadMaterialObservaciones : null,

            disponibilidadEquipo:                 seguimiento.disponibilidadEquipoCodigo !== undefined ? seguimiento.disponibilidadEquipoCodigo : null,
            causaEquipo:                          seguimiento.causaIndisponibilidadEquipoCodigo !== undefined ? seguimiento.causaIndisponibilidadEquipoCodigo : null,
            retrasoEquipo:                        seguimiento.seGeneroRetrasoEquipo !== undefined ? seguimiento.seGeneroRetrasoEquipo : null,
            horasRetrasoEquipo:                   seguimiento.numeroHorasRetrasoEquipo !== undefined ? seguimiento.numeroHorasRetrasoEquipo : null,
            disponibilidadEquipoObservaciones:    seguimiento.disponibilidadEquipoObservaciones !== undefined ? seguimiento.disponibilidadEquipoObservaciones : null,

            Productividad:                        seguimiento.productividadCodigo !== undefined ? seguimiento.productividadCodigo : null,
            causaProductividad:                   seguimiento.causaIndisponibilidadProductividadCodigo !== undefined ? seguimiento.causaIndisponibilidadProductividadCodigo : null,
            retrasoProductividad:                 seguimiento.seGeneroRetrasoProductividad !== undefined ? seguimiento.seGeneroRetrasoProductividad : null,
            horasRetrasoProductividad:            seguimiento.numeroHorasRetrasoProductividad !== undefined ? seguimiento.numeroHorasRetrasoProductividad : null,
            ProductividadObservaciones:           seguimiento.productividadObservaciones !== undefined ? seguimiento.productividadObservaciones : null,
          }
        )
        console.log(this.diasPermitidos, new Date( this.addressForm.value.fechaSeguimiento ).toLocaleDateString());
        this.diasPermitidos.push( new Date( this.addressForm.value.fechaSeguimiento ).toLocaleDateString() )

      });
  }

  diasPermitidos = [];

  filtroCalendario = (d: Date | null): boolean => {
    const day = (d || new Date()).getDay();
    const today = new Date();

    if ( environment.production === true ){
      return ( 
              this.diasPermitidos.includes( new Intl.DateTimeFormat(['ban', 'id']).format(d)) && 
              ( d <= today ) 
      );
    }else{
      return ( 
              this.diasPermitidos.includes( new Intl.DateTimeFormat(['ban', 'id']).format(d))
        );
    }
    
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    //console.log(this.addressForm.value);
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    let values = this.addressForm.value;

    let seguimiento: SeguimientoDiario = {
      fechaSeguimiento:                     values.fechaSeguimiento,
      contratacionProyectoId:               this.proyecto.contratacionProyectoId,
      seguimientoDiarioId:                  this.seguimientoId,

      disponibilidadPersonal:               values.disponibilidadPersonal,
      cantidadPersonalProgramado:           values.cantidadPersonalOperativoProgramado,
      cantidadPersonalTrabajando:           values.cantidadPersonalOperativoTrabajando,
      seGeneroRetrasoPersonal:              values.retrasoPersonal,
      numeroHorasRetrasoPersonal:           values.horasRetrasoPersonal,
      disponibilidadPersonalObservaciones:  values.disponibilidadPersonalObservaciones,
      
      disponibilidadMaterialCodigo:         values.disponibilidadMaterial,
      causaIndisponibilidadMaterialCodigo:  values.causaMaterial,
      seGeneroRetrasoMaterial:              values.retrasoMaterial,
      numeroHorasRetrasoMaterial:           values.horasRetrasoMaterial,
      disponibilidadMaterialObservaciones:  values.disponibilidadMaterialObservaciones,
      
      disponibilidadEquipoCodigo:           values.disponibilidadEquipo,
      causaIndisponibilidadEquipoCodigo:    values.causaEquipo,
      seGeneroRetrasoEquipo:                values.retrasoEquipo,
      numeroHorasRetrasoEquipo:             values.horasRetrasoEquipo,
      disponibilidadEquipoObservaciones:    values.disponibilidadEquipoObservaciones,
      
      productividadCodigo:                  values.Productividad,
      causaIndisponibilidadProductividadCodigo:  values.causaProductividad,
      seGeneroRetrasoProductividad:         values.retrasoProductividad,
      numeroHorasRetrasoProductividad:      values.horasRetrasoProductividad,
      productividadObservaciones:           values.ProductividadObservaciones,

    } 

    console.log(seguimiento);

    this.dailyFollowUpService.createEditDailyFollowUp( seguimiento )
      .subscribe( respuesta => {
        this.openDialog('', respuesta.message);
        if ( respuesta.code == "200" )
          this.router.navigate(['registroSeguimientoDiario'])
      });

  }
}

