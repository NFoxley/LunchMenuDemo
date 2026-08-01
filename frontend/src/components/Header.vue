<script setup>
import { ref } from 'vue'
</script>

<template>
    <header>
        <nav class="flex">
            <div
                class="logo flex h-headerNav w-12 items-center justify-center motion-safe:transition-colors bg-gold text-brand-black">
                <svg xmlns="http://www.w3.org/2000/svg" class="w-[26px]" viewBox="0 0 26 51" fill="currentColor">
                    <g clip-path="url(#clip0_2968_58176)">
                        <path
                            d="M1.52648 1.52648H24.4673V12.2305H19.8816V38.2305H24.4673V48.9346H1.52648V38.2305H6.11215V12.2368H1.52648V1.52648ZM0 0V13.7632H4.58567V36.704H0V50.4673H26V36.704H21.4143V13.7632H26V0H0Z">
                        </path>
                        <path
                            d="M7.64487 39.7636H3.0592V47.4085H22.9408V39.7636H18.3489V10.7044H22.9408V3.05957H3.0592V10.7044H7.64487V39.7636Z">
                        </path>
                    </g>
                    <defs>
                        <clipPath id="clip0_2968_58176">
                            <rect width="26" height="50.4673"></rect>
                        </clipPath>
                    </defs>
                </svg>
            </div>
            <div class="flex-expand search">
                <div class="absolute opacity-85 transition-all md:left-2 md:right-auto right-6">
                    <button type="submit" aria-label="Search" class="block">
                        <svg xmlns="http://www.w3.org/2000/svg" class="size-6" fill="none" viewBox="0 0 31 30">
                            <path fill="currentColor"
                                d="M27.5136 30L16.9656 19.4924C16.1904 20.1072 15.2747 20.5949 14.2184 20.9555C13.1621 21.3168 12.0323 21.4975 10.8291 21.4975C7.80787 21.4975 5.24771 20.4479 3.14862 18.3488C1.04954 16.2498 0 13.7162 0 10.7483C0 7.75396 1.04954 5.21402 3.14862 3.12841C5.24771 1.0428 7.79439 0 10.7887 0C13.7836 0 16.3239 1.0428 18.4095 3.12841C20.4951 5.21402 21.5379 7.75396 21.5379 10.7483C21.5379 11.9252 21.364 13.035 21.0162 14.0778C20.669 15.1206 20.1611 16.0698 19.4924 16.9252L30.0809 27.4732L27.5136 30ZM10.7887 17.9677C12.7941 17.9677 14.4921 17.2659 15.8827 15.8625C17.2733 14.4584 17.9686 12.7536 17.9686 10.7483C17.9686 8.74286 17.2733 7.03844 15.8827 5.63499C14.4921 4.23089 12.7941 3.52885 10.7887 3.52885C8.78329 3.52885 7.07887 4.23089 5.67542 5.63499C4.27132 7.03844 3.56927 8.74286 3.56927 10.7483C3.56927 12.7536 4.27132 14.4584 5.67542 15.8625C7.07887 17.2659 8.78329 17.9677 10.7887 17.9677Z">
                            </path>
                        </svg>
                    </button>
                </div>
                Search
            </div>
            <ul>
                <!-- <li><a>Search</a></li> -->
                <li><a>Explore</a></li>
                <li><a>Costs</a></li>
                <li><a>Admissions</a></li>
            </ul>
        </nav>
    </header>
</template>