import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit, TemplateRef } from '@angular/core';

@Component({
  selector: 'app-stepper',
  templateUrl: './stepper.component.html',
  styleUrls: ['./stepper.component.css'],
  providers: [{provide: CdkStepper, useExisting: StepperComponent}]
})
export class StepperComponent extends CdkStepper implements OnInit {
@Input() linerModeSelected: boolean;

theSelected: any;

  ngOnInit() {
    // super.ngAfterContentInit();

    // this.linear = this.linerModeSelected;
    // this.theSelected = this.selected?.content;
    console.log(this.selected)
  }

  override ngAfterContentInit(): void {
    super.ngAfterContentInit();

    this.linear = this.linerModeSelected;
    this.theSelected = this.selected?.content;
    console.log(this.selected)
  }

  onClick(index: number) {
    this.selectedIndex = index;
    console.log(index);
    console.log(this.selectedIndex);

  }

  // onClick(index: number) {
  //   this.selectedIndex = index--;
  //   console.log(index);
  //   console.log(this.selectedIndex);

  // }

}
